using System;
using UnityEditor;
using UnityEngine;
using Bitverse.Unity.Gui;

public class BitControlEditor : Editor
{
    enum Corner
    {
        None,
        Top, Left, Bottom, Right,
        TopLeft, TopRight, BottomLeft, BottomRight
    }


    public void Awake()
    {
    }

    private void Callback()
    {
        Debug.Log("funfa");
    }

    private BitControl underMouse;
    public static bool SnapToGrid = true;
    public static Vector2 Grid = new Vector2(4, 4);
    public Vector2 mouseDownPosition;
    private Corner mouseCorner = Corner.None;
    private Rect originalPosition;
    private bool SnapToControls = true;
    Vector3 line1p1 = Vector3.zero;
    Vector3 line1p2 = Vector3.zero;
    Vector3 line2p1 = Vector3.zero;
    Vector3 line2p2 = Vector3.zero;
    private static bool delayedMouseDown = false;

    public void OnSceneGUI()
    {
        if (target == null)
            return;
        BitControl c = (BitControl)target;
        Handles.BeginGUI();
        Rect r = ToGuiRect(c.AbsolutePosition);
        float rm = 6;
        float rmh = rm / 2;
        Corner corner = Corner.None;
        corner = CornerStuff(corner, Corner.TopLeft, new Rect(r.xMin - rmh, r.yMin - rmh, rm, rm), MouseCursor.ResizeUpLeft);
        corner = CornerStuff(corner, Corner.TopRight, new Rect(r.xMax - rmh, r.yMin - rmh, rm, rm), MouseCursor.ResizeUpRight);
        corner = CornerStuff(corner, Corner.BottomRight, new Rect(r.xMax - rmh, r.yMax - rmh, rm, rm), MouseCursor.ResizeUpLeft);
        corner = CornerStuff(corner, Corner.BottomLeft, new Rect(r.xMin - rmh, r.yMax - rmh, rm, rm), MouseCursor.ResizeUpRight);

        corner = CornerStuff(corner, Corner.Left, new Rect(r.xMin - rmh, r.yMin + rmh, rm, r.height - rm), MouseCursor.ResizeHorizontal);
        corner = CornerStuff(corner, Corner.Right, new Rect(r.xMax - rmh, r.yMin + rmh, rm, r.height - rm), MouseCursor.ResizeHorizontal);
        corner = CornerStuff(corner, Corner.Top, new Rect(r.xMin + rmh, r.yMin - rmh, r.width - rm, rm), MouseCursor.ResizeVertical);
        corner = CornerStuff(corner, Corner.Bottom, new Rect(r.xMin + rmh, r.yMax - rmh, r.width - rm, rm), MouseCursor.ResizeVertical);
        Handles.EndGUI();

        object[] comps = FindObjectsOfType(typeof(BitControl));

        if (Event.current.type == EventType.MouseUp)
        {
            line1p1 = line1p2 = line2p1 = line2p2 = new Vector3(10000, 10000, 10000);
        }
        if (delayedMouseDown)
        {
            mouseCorner = corner;
            originalPosition = c.AbsolutePosition;
            //Debug.Log("a = " + originalPosition);
        }


        if (Event.current.type == EventType.KeyDown)
            if (Event.current.keyCode == KeyCode.LeftControl)
                SnapToControls = false;
        if (Event.current.type == EventType.KeyUp)
            if (Event.current.keyCode == KeyCode.LeftControl)
                SnapToControls = true;
        if (Event.current.type == EventType.MouseMove)
        {
            BitControl n = GetCompUnderMouse(comps);
            if (n != underMouse)
            {
                underMouse = n;
                if (underMouse != null)
                    EditorUtility.SetDirty(underMouse);
            }
        }

        if (corner == Corner.None && delayedMouseDown && Event.current.button == 0)
        {
            BitControl best = GetCompUnderMouse(comps);
            if (best != null && best != c)
            {
                Selection.activeGameObject = best.gameObject;
                return;
            }
            if (best == null)
            {
                Selection.activeGameObject = c.gameObject;
            }

        }
        delayedMouseDown = false;
        if (Event.current.type == EventType.MouseDown)
        {
            delayedMouseDown = true;
            BitControl best = GetCompUnderMouse(comps);
            if (best != null && corner == Corner.None)
            {
                originalPosition = best.AbsolutePosition;
                //Debug.Log("a = " + originalPosition);
            }
        }

        Vector3 op = new Vector3(originalPosition.x, 0, originalPosition.y);
        Vector3 pos = Handles.FreeMoveHandle(op, Quaternion.identity, 10000, new Vector3(1, 1, 1), Handles.RectangleCap);
        Handles.color = Color.yellow;
        Handles.DrawLine(op, pos);
        Handles.DrawLine(op - 4 * Vector3.forward, op + 4 * Vector3.forward);
        Handles.DrawLine(op - 4 * Vector3.right, op + 4 * Vector3.right);
        Handles.DrawLine(pos - 4 * Vector3.forward, pos + 4 * Vector3.forward);
        Handles.DrawLine(pos - 4 * Vector3.right, pos + 4 * Vector3.right);
        if (op == pos)
        {
            //Debug.Log("SSSS");
        }
        if (op != pos)
        {
            pos = DoSnapping(pos, c, comps, mouseCorner);
            Vector3 dif = pos - op;
            //Debug.Log("AAAA" + op + " " + b + " " + pos + " " + dif);

            switch (mouseCorner)
            {
                case Corner.None:
                    c.AbsolutePosition = new Rect(dif.x + originalPosition.x, dif.z + originalPosition.y, originalPosition.width, originalPosition.height);
                    break;
                case Corner.Right:
                    c.Size = new Bitverse.Unity.Gui.Size(dif.x + originalPosition.width, originalPosition.height);
                    break;
                case Corner.Left:
                    c.AbsolutePosition = new Rect(dif.x + originalPosition.x, originalPosition.y, originalPosition.width - dif.x, c.Size.Height);
                    break;
                case Corner.Bottom:
                    c.Size = new Size(originalPosition.width, dif.z + originalPosition.height);
                    break;
                case Corner.Top:
                    c.AbsolutePosition = new Rect(originalPosition.x, dif.z + originalPosition.y, c.Size.Width, -dif.z + originalPosition.height);
                    break;
                case Corner.BottomRight:
                    c.Size = new Size(dif.x + originalPosition.width, dif.z + originalPosition.height);
                    break;
                case Corner.TopLeft:
                    c.AbsolutePosition = new Rect(dif.x + originalPosition.x, dif.z + originalPosition.y, -dif.x + originalPosition.width, -dif.z + originalPosition.height);
                    break;
                case Corner.TopRight:
                    c.AbsolutePosition = new Rect(originalPosition.x, dif.z + originalPosition.y, dif.x + originalPosition.width, -dif.z + originalPosition.height);
                    break;
                case Corner.BottomLeft:
                    c.AbsolutePosition = new Rect(dif.x + originalPosition.x, originalPosition.y, -dif.x + originalPosition.width, dif.z + originalPosition.height);
                    break;
            }
            if (GUI.changed)
                EditorUtility.SetDirty(target);

        }

        foreach (BitControl comp in comps)
        {
            if (comp == underMouse)
            {
                Handles.color = Color.yellow;
                DrawBorder(comp);
            }
            if (comp == c)
            {
                Handles.color = Color.blue;
                DrawBorder(comp);
            }
            else
            {
                EditorGUIUtility.AddCursorRect(ToGuiRect(comp.AbsolutePosition), MouseCursor.Link);
            }
            //comp.Draw();
            Handles.color = Color.white;
            Handles.Label(new Vector3(comp.AbsolutePosition.x, 0, comp.AbsolutePosition.y), comp.gameObject.name);
        }
        Handles.color = Color.blue;
        Handles.DrawLine(line1p1, line1p2);
        Handles.DrawLine(line2p1, line2p2);

    }

    private Vector3 DoSnapping(Vector3 pos, BitControl control, object[] comps, Corner corner)
    {
        Vector3 changedPos = pos;
        float bestx = float.MaxValue;
        float besty = float.MaxValue;
        if (SnapToControls)
        {
            foreach (BitControl comp in comps)
            {
                if (comp == control)
                    continue;
                float dif = 0;
                if (corner == Corner.Left || corner == Corner.None || corner == Corner.TopLeft || corner == Corner.BottomLeft)
                {
                    float delta = Math.Abs(comp.AbsolutePosition.x - pos.x);
                    if (delta < 10 && delta < bestx)
                    {
                        changedPos.x = comp.AbsolutePosition.x;
                        bestx = delta;
                        line1p1 = new Vector3(changedPos.x, 0, Math.Min(comp.AbsolutePosition.yMin, control.AbsolutePosition.yMin));
                        line1p2 = new Vector3(changedPos.x, 0, Math.Max(comp.AbsolutePosition.yMax, control.AbsolutePosition.yMax));
                    }
                }
                if (corner == Corner.Top || corner == Corner.None || corner == Corner.TopLeft || corner == Corner.TopRight)
                {
                    float delta = Math.Abs(comp.AbsolutePosition.y - pos.z);
                    if (delta < 10 && delta < besty)
                    {
                        //Debug.Log("!!!!" + pos + " -  " + comp.AbsolutePosition);

                        changedPos.z = comp.AbsolutePosition.y;
                        besty = delta;
                        line2p1 = new Vector3(Math.Min(comp.AbsolutePosition.xMin, control.AbsolutePosition.xMin), 0, changedPos.z);
                        line2p2 = new Vector3(Math.Max(comp.AbsolutePosition.xMax, control.AbsolutePosition.xMax), 0, changedPos.z);
                    }
                }
                if (corner == Corner.Right || corner == Corner.None || corner == Corner.TopRight || corner == Corner.BottomRight)
                {
                    float delta = Math.Abs(dif = comp.AbsolutePosition.xMax - (pos.x + originalPosition.width));
                    if (delta < 10 && delta < bestx)
                    {
                        changedPos.x = pos.x + dif;
                        bestx = delta;
                        line1p1 = new Vector3(comp.AbsolutePosition.xMax, 0, Math.Min(comp.AbsolutePosition.yMin, control.AbsolutePosition.yMin));
                        line1p2 = new Vector3(comp.AbsolutePosition.xMax, 0, Math.Max(comp.AbsolutePosition.yMax, control.AbsolutePosition.yMax));
                    }
                }
                if (corner == Corner.Bottom || corner == Corner.None || corner == Corner.BottomLeft || corner == Corner.BottomRight)
                {
                    float delta = Math.Abs(dif = comp.AbsolutePosition.yMax - (pos.z + originalPosition.height));
                    if (delta < 10 && delta < besty)
                    {
                        changedPos.z = pos.z + dif;
                        besty = delta;
                        line2p1 = new Vector3(Math.Min(comp.AbsolutePosition.xMin, control.AbsolutePosition.xMin), 0, comp.AbsolutePosition.yMax);
                        line2p2 = new Vector3(Math.Max(comp.AbsolutePosition.xMax, control.AbsolutePosition.xMax), 0, comp.AbsolutePosition.yMax);
                    }
                }
            }
        }
        if (SnapToGrid)
        {
            if (bestx == float.MaxValue) changedPos.x = changedPos.x - (changedPos.x % Grid.x);
            if (besty == float.MaxValue) changedPos.z = changedPos.z - (changedPos.z % Grid.y);
        }
        return changedPos;
    }

    private Corner CornerStuff(Corner currentCorner, Corner corner, Rect rect, MouseCursor cursor)
    {
        EditorGUIUtility.AddCursorRect(rect, cursor);
        if (rect.Contains(Event.current.mousePosition))
        {
            return corner;
        }
        return currentCorner;
    }

    internal Rect ToGuiRect(Rect r)
    {
        Vector2 v = HandleUtility.WorldToGUIPoint(new Vector3(r.x, 0, r.y));
        Vector2 z = HandleUtility.WorldToGUIPoint(new Vector3(0, 0, 0));
        Vector2 s = HandleUtility.WorldToGUIPoint(new Vector3(r.width, 0, r.height));
        return new Rect(v.x, v.y, s.x - z.x, s.y - z.y);
    }

    internal Rect FromGuiRect(Rect r)
    {
        Vector2 v = HandleUtility.GUIPointToWorldRay(new Vector3(r.x, 0, r.y)).origin;
        Vector2 z = HandleUtility.GUIPointToWorldRay(new Vector3(0, 0, 0)).origin;
        Vector2 s = HandleUtility.GUIPointToWorldRay(new Vector3(r.width, 0, r.height)).origin;
        return new Rect(v.x, v.y, s.x - z.x, s.y - z.y);
    }

    private void DrawBorder(BitControl comp)
    {
        Rect cp = comp.AbsolutePosition;
        float m = 1;
        Handles.DrawPolyLine(new Vector3[]
                                 {
                                     new Vector3(cp.xMin+m, 0, cp.yMin+m), 
                                     new Vector3(cp.xMax-m, 0, cp.yMin+m), 
                                     new Vector3(cp.xMax-m, 0, cp.yMax-m), 
                                     new Vector3(cp.xMin+m, 0, cp.yMax-m), 
                                     new Vector3(cp.xMin+m, 0, cp.yMin+m), 
                                 });
    }


    private BitControl GetCompUnderMouse(object[] comps)
    {
        Ray r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector2 mousePosition = new Vector2(r.origin.x, r.origin.z);
        BitControl best = null;
        float bestDist = float.MaxValue;
        foreach (BitControl comp in comps)
        {
            if (comp.AbsolutePosition.Contains(mousePosition))
            {
                float mx = mousePosition.x;
                float my = mousePosition.y;
                float d = Math.Min(Math.Abs(comp.AbsolutePosition.xMin - mx), Math.Abs(comp.AbsolutePosition.xMax - mx));
                d = Math.Min(Math.Abs(comp.AbsolutePosition.yMin - my), d);
                d = Math.Min(Math.Abs(comp.AbsolutePosition.yMax - my), d);
                if (d < bestDist)
                {
                    best = comp;
                    bestDist = d;
                }
            }
        }
        return best;
    }

}

[CustomEditor(typeof(BitButton))]
public class BitButtonEditor : BitControlEditor { }
[CustomEditor(typeof(BitBox))]
public class BitBoxEditor : BitControlEditor { }
[CustomEditor(typeof(BitControl))]
public class BitControlEditor2 : BitControlEditor { }
[CustomEditor(typeof(BitLabel))]
public class BitLabelEditor : BitControlEditor { }
[CustomEditor(typeof(BitPasswordField))]
public class BitPasswordFieldEditor : BitControlEditor { }
[CustomEditor(typeof(BitRepeatButton))]
public class BitRepeatButtonEditor : BitControlEditor { }
[CustomEditor(typeof(BitScrollView))]
public class BitScrollViewEditor : BitControlEditor { }
[CustomEditor(typeof(BitTextArea))]
public class BitTextAreaEditor : BitControlEditor { }
[CustomEditor(typeof(BitDrawTexture))]
public class BitDrawTextureEditor : BitControlEditor { }
[CustomEditor(typeof(BitGroup))]
public class BitGroupEditor : BitControlEditor { }
[CustomEditor(typeof(BitTextField))]
public class BitTextFieldEditor : BitControlEditor { }
[CustomEditor(typeof(BitHorizontalScrollbar))]
public class BitHorizontalScrollbarEditor : BitControlEditor { }
[CustomEditor(typeof(BitHorizontalSlider))]
public class BitHorizontalSliderEditor : BitControlEditor { }
[CustomEditor(typeof(BitToggle))]
public class BitToggleEditor : BitControlEditor { }
[CustomEditor(typeof(BitWindow))]
public class BitWindowEditor : BitControlEditor { }
[CustomEditor(typeof(BitVerticalScrollbar))]
public class BitVerticalScrollbarEditor : BitControlEditor { }
[CustomEditor(typeof(BitVerticalSlider))]
public class BitXVerticalSliderEditor : BitControlEditor { }
[CustomEditor(typeof(BitList))]
public class BitListEditor : BitControlEditor { }
