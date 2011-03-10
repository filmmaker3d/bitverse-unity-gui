using Bitverse.Unity.Gui;
using UnityEngine;


[RequireComponent(typeof(BitWindow))]
public abstract class TooltipProvider : MonoBehaviour
{
    public abstract string ProviderName();

    public abstract void PreloadTooltip(BitControl control);

    public abstract bool ShowTooltip(BitControl control, Point position);

    public abstract bool HideTooltip(BitControl control);


    public static object GetUserProperty(BitControl control, string name)
    {
        if ((control == null) || (control.UserProperties == null))
        {
            return null;
        }

        return control.UserProperties.ContainsKey(name) ? control.UserProperties[name] : null;
    }


    public static object GetUserProperty(BitControl control, string name, object defaultValue)
    {
        return control.UserProperties.ContainsKey(name) ? control.UserProperties[name] : defaultValue;
    }


    public static void Show(BitWindow window, Point position)
    {
        window.ShowAsTooltip(position);
        window.FormMode = FormModes.Popup;
        window.Visible = true;
    }
}
