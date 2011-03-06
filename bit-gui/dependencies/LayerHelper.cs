using UnityEngine;


public class LayerHelper
{
    public enum LayerContext
    {
        Radar,
        Raycast,
        RadarAndRaycast,
        Map,
        ModelViewer,
        PanoramaObjects,
        Monitor,
        Machinima,
        Default,
        MiniMap
    } ;

    public static int GetLayers(LayerContext c)
    {
        int layer;
        switch (c)
        {
            case LayerContext.Radar:
                layer = LayerMask.NameToLayer("Radar");
                break;
            case LayerContext.Machinima:
                layer = LayerMask.NameToLayer("Machinima");
                break;
            case LayerContext.Raycast:
                layer = LayerMask.NameToLayer("Ignore Raycast");
                break;
            case LayerContext.RadarAndRaycast:
                layer = LayerMask.NameToLayer("Radar") | LayerMask.NameToLayer("Ignore Raycast"); 
                break;
            case LayerContext.Map:
                layer = LayerMask.NameToLayer("Map");
                break;
            case LayerContext.ModelViewer:
                layer = LayerMask.NameToLayer("ModelViewer");
                break;
            case LayerContext.PanoramaObjects:
                layer = LayerMask.NameToLayer("PanoramaObjects");
                break;
            case LayerContext.Monitor:
                layer = LayerMask.NameToLayer("Monitor");
                break;
            case LayerContext.MiniMap:
                layer = LayerMask.NameToLayer("MiniMap");
                break;
            default:
                layer = LayerMask.NameToLayer("Default");
                break;
        }
        return layer;
    }
}