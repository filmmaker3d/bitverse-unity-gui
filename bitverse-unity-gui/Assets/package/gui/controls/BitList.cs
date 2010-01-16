using UnityEngine;
using Bitverse.Unity.Gui;


public class BitList : BitControl
{
    protected BitControl GetRenderer()
    {
        for (int i = 0, count = transform.childCount; i < count; i++)
        {
            Transform ch = transform.GetChild(i);
            BitControl c = (BitControl)ch.GetComponent(typeof(BitControl));
            if (c != null) return c;
        }
        return null;
    }


    public override void DoDraw()
    {
        object[] arr = new object[] { "orange", "apple", "lemon" };
        BitControl r = GetRenderer();

        if (r == null)
            return;

        Rect cp = this.Position;
        Rect rp = r.Position;
        if (Style != null)
        {
            GUI.BeginGroup(Position, Content, Style);
        }
        else
        {
            GUI.BeginGroup(Position, Content);
            GUI.Box(new Rect(0, 0, cp.width, cp.height), Content);
        }
        for (int i = 0, s = arr.Length; i < s; i++)
        {
            Rect p = rp;
            p.y = i * p.height;
            //p.width = cp.width;
            r.Position = new Rect(0, 0, p.width, p.height);
            GUI.BeginGroup(p);
            r.Content.text = arr[i].ToString();
            if (GUI.Button(r.Position, "", new GUIStyle()))
            {
                Debug.Log("Bla");
            }
            r.Draw();


            //int x = GUIUtility.GetControlID(i, FocusType.Keyboard, p);
            //EventType t = Event.current.GetTypeForControl(x);
            //if (t != EventType.Repaint && t != EventType.Layout && t != EventType.ignore)
            //    Debug.Log(x + " " + t);
            GUI.EndGroup();
        }
        GUI.EndGroup();
        r.Position = rp;
    }

}
