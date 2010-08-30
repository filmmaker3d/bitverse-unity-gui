using Bitverse.Unity.Gui;
using UnityEngine;


[RequireComponent(typeof(BitWindow))]
public class TextTooltipProvider : TooltipProvider
{
    private BitLabel _messageLabel;

    private BitWindow _window;


    public override string ProviderName()
    {
        return "text_tooltip";
    }


    public override bool ShowTooltip(BitControl control, Point position)
    {
        if (control == null)
        {
            return false;
        }

        bool dynamicTooltip = control.UserProperties.ContainsKey("currentResult");

        if ((string.IsNullOrEmpty(control.Content.tooltip)) && (!dynamicTooltip))
        {
            return false;
        }

        if (_window == null)
        {
            _window = GetComponent<BitWindow>();
            _messageLabel = _window.FindControl<BitLabel>("message_label");
        }

        if (dynamicTooltip)
        {
            _messageLabel.Text = "The next result will be " + control.UserProperties["currentResult"];
        }
        else
        {
            _messageLabel.Text = control.Content.tooltip;
        }

        Show(_window, position);
        return true;
    }


    public override bool HideTooltip(BitControl control)
    {
        if (_window != null)
        {
            _window.Visible = false;
        }

        return true;
    }
}
