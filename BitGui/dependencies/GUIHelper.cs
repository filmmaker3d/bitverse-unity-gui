using UnityEngine;


public class GUIHelper
{
    public static bool clippingEnabled;
    protected static Rect clippingBounds;
    protected static Material lineMaterial;

    /* @ Credit: "http://cs-people.bu.edu/jalon/cs480/Oct11Lab/clip.c" */
    protected static bool clip_test(float p, float q, ref float u1, ref float u2)
    {
        float r;
        bool retval = true;
        if (p < 0.0)
        {
            r = q / p;
            if (r > u2)
                retval = false;
            else if (r > u1)
                u1 = r;
        }
        else if (p > 0.0)
        {
            r = q / p;
            if (r < u1)
                retval = false;
            else if (r < u2)
                u2 = r;
        }
        else
            if (q < 0.0)
                retval = false;

        return retval;
    }

    public static IntersectStruct Intersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        IntersectStruct response = new IntersectStruct();
        Vector2 b = a2 - a1;
        Vector2 d = b2 - b1;
        float bDotDPerp = b.x * d.y - b.y * d.x;

        // if b dot d == 0, it means the lines are parallel so have infinite intersection points
        if (bDotDPerp == 0)
            return response;

        Vector2 c = b1 - a1;
        float t = (c.x * d.y - c.y * d.x) / bDotDPerp;
        if (t < 0 || t > 1)
            return response;

        float u = (c.x * b.y - c.y * b.x) / bDotDPerp;
        if (u < 0 || u > 1)
            return response;

        response.Valid = true;
        response.Point = a1 + t * b;

        return response;
    }

    public struct IntersectStruct
    {
        public bool Valid;
        public Vector2 Point;
    }

    public static IntersectStruct IntersectRectangle(Vector2 sp, Vector2 tp, Rect source)
    {
        GUIHelper.IntersectStruct response = Intersects(new Vector2(sp.x, sp.y), new Vector2(tp.x, tp.y), new Vector2(source.x, source.y + source.height),
                                                                              new Vector2(source.x + source.width, source.y + source.height));
        if (!response.Valid)
            response = Intersects(new Vector2(sp.x, sp.y), new Vector2(tp.x, tp.y), new Vector2(source.x, source.y),
                                                                  new Vector2(source.x + source.width, source.y));
        if (!response.Valid)
            response = Intersects(new Vector2(sp.x, sp.y), new Vector2(tp.x, tp.y), new Vector2(source.x + source.width, source.y),
                                                                  new Vector2(source.x + source.width, source.y + source.height));
        if (!response.Valid)
            response = Intersects(new Vector2(sp.x, sp.y), new Vector2(tp.x, tp.y), new Vector2(source.x, source.y),
                                                                  new Vector2(source.x, source.y + source.height));
        return response;
    }


    protected static bool segment_rect_intersection(Rect bounds, ref Vector2 p1, ref Vector2 p2)
    {
        float u1 = 0.0f, u2 = 1.0f, dx = p2.x - p1.x, dy;
        if (clip_test(-dx, p1.x - bounds.xMin, ref u1, ref u2))
            if (clip_test(dx, bounds.xMax - p1.x, ref u1, ref u2))
            {
                dy = p2.y - p1.y;
                if (clip_test(-dy, p1.y - bounds.yMin, ref u1, ref u2))
                    if (clip_test(dy, bounds.yMax - p1.y, ref u1, ref u2))
                    {
                        if (u2 < 1.0)
                        {
                            p2.x = p1.x + u2 * dx;
                            p2.y = p1.y + u2 * dy;
                        }
                        if (u1 > 0.0)
                        {
                            p1.x += u1 * dx;
                            p1.y += u1 * dy;
                        }
                        return true;
                    }
            }
        return false;
    }

    public static void BeginGroup(Rect position)
    {
        clippingEnabled = true;
        clippingBounds = new Rect(0, 0, position.width, position.height);
        GUI.BeginGroup(position);
    }

    public static void EndGroup()
    {
        GUI.EndGroup();
        clippingBounds = new Rect(0, 0, Screen.width, Screen.height);
        clippingEnabled = false;
    }

    public static void DrawRect(Rect rect, Color color)
    {
        DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x, rect.yMax), color);
        DrawLine(new Vector2(rect.xMax, rect.y), new Vector2(rect.xMax, rect.yMax), color);
        DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.xMax, rect.y), color);
        DrawLine(new Vector2(rect.x, rect.yMax), new Vector2(rect.xMax, rect.yMax), color);
    }

    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color)
    {
        if (clippingEnabled)
            if (!segment_rect_intersection(clippingBounds, ref pointA, ref pointB))
                return;

        if (!lineMaterial)
        {
            /* Credit:  */
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
           "SubShader { Pass {" +
           "   BindChannels { Bind \"Color\",color }" +
           "   Blend SrcAlpha OneMinusSrcAlpha" +
           "   ZWrite Off Cull Off Fog { Mode Off }" +
           "} } }");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }

        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);
        GL.Color(color);
        GL.Vertex3(pointA.x, pointA.y, 0);
        GL.Vertex3(pointB.x, pointB.y, 0);
        GL.End();

    }

    public static bool IsOver3DPlane(Camera camera, GameObject plane)
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(camera.ScreenPointToRay(Input.mousePosition), float.MaxValue);
        if (raycastHits == null || raycastHits.Length == 0)
            return false;
        float dist = -1;
        GameObject closest = null;
        foreach (RaycastHit ray in raycastHits)
        {
            if (ray.distance < dist || dist == -1)
            {
                closest = ray.rigidbody.gameObject;
            }
        }
        if (closest == null)
            return false;
        return (plane.name == closest.name);
    }

    public static GameObject CreateSimplePlane(string name, Transform parent)
    {
        GameObject plane = new GameObject();
        MeshFilter mymeshfilter = plane.AddComponent<MeshFilter>();
        plane.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        mymeshfilter.mesh = mesh;
        mesh.vertices = new Vector3[] { new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(1, 1, 0), new Vector3(-1, 1, 0) };
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        mesh.RecalculateBounds();
        plane.name = name;
        plane.transform.parent = parent;
        plane.transform.localPosition = new Vector3(0, 0, 0);
        plane.transform.rotation = new Quaternion();
        plane.renderer.material.color = new Color(1, 1, 1, 1);
        return plane;
    }

    public static GameObject CreateLabel(string name, Transform parent, Vector3 scale, Vector3 position, Vector3 rotation, string text)
    {
        return CreateLabel(name, parent, scale, position, rotation, text, (Font)Resources.Load("Menu3D/fonts/xirod24"));
    }

    public static GameObject CreateLabel(string name, Transform parent, Vector3 scale, Vector3 position, Vector3 rotation, string text, Font font)
    {
        GameObject label = new GameObject(name);
        label.AddComponent<MeshRenderer>();
        ChangeText(label, text, font);
        label.gameObject.transform.parent = parent;
        label.transform.localScale = scale;
        label.transform.localRotation = Quaternion.Euler(rotation);
        label.transform.localPosition = position;

        return label;
    }

    public static TextMesh ChangeText(GameObject label, string text, Font font)
    {
        TextMesh textmesh = label.GetComponent<TextMesh>();
        if (textmesh != null)
            GameObject.DestroyImmediate(textmesh);
        textmesh = label.AddComponent<TextMesh>();
        textmesh.font = font;
        textmesh.renderer.material = textmesh.font.material;
        textmesh.text = text;
        return textmesh;
    }

    public static Vector2 DistanceFromMouse(Vector2 position)
    {
        Vector2 itempos = position;
        Vector2 mousepos = Input.mousePosition;
        return new Vector2(Mathf.Abs(itempos.x - mousepos.x), Mathf.Abs(itempos.y - mousepos.y));
    }
};