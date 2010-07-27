using System.Collections.Generic;
using System.Timers;
using bitgui;
using Bitverse.Unity.Gui;
using UnityEngine;


public class TooltipManager : MonoBehaviour
{
    public string TextTooltipProviderName = "text_tooltip";

    public double ShowTooltipDelay = 1000.0;

    public double HideTooltipDelay = 10000.0;

    public readonly List<TooltipProvider> Providers = new List<TooltipProvider>();

    private BitControl _currentTooltipControl;

    private Timer _showTooltipTimer;

    private Timer _hideTooltipTimer;

    private int _currentTooltipPathHash;

    private TooltipProvider _currentTooltipProvider;


    public void Start()
    {
        _showTooltipTimer = new Timer(ShowTooltipDelay);
        _showTooltipTimer.Elapsed += OnShowTimer;
        _showTooltipTimer.Enabled = false;

        _hideTooltipTimer = new Timer(HideTooltipDelay);
        _hideTooltipTimer.Elapsed += OnHideTimer;
        _hideTooltipTimer.Enabled = false;
    }


    /// <summary>
    /// Imediately shows the control tooltip
    /// </summary>
    /// <param name="control">Source control with an ITooltipProvider</param>
    /// <param name="position"></param>
    /// <returns>true if a tooltip was shown</returns>
    public bool ShowTooltip(BitControl control, Point position)
    {
        bool showSuccessful = _currentTooltipProvider != null && _currentTooltipProvider.ShowTooltip(control, position);

        if (showSuccessful)
        {
            _hideTooltipTimer.Start();
        }

        return showSuccessful;
    }


    public bool HideTooltip()
    {
        return HideTooltip(_currentTooltipControl);
    }


    /// <summary>
    /// Imediately hides the control tooltip
    /// </summary>
    /// <param name="control">Source control with an ITooltipProvider</param>
    /// <returns>true if the tooltip was hidden</returns>
    public bool HideTooltip(BitControl control)
    {
        if ((_currentTooltipProvider != null) && (_currentTooltipProvider.HideTooltip(control)))
        {
            _hideTooltipTimer.Stop();
            return true;
        }

        return false;
    }


    public void BeginHover(BitControl control)
    {
        EndHover(_currentTooltipControl);

        _currentTooltipControl = control;
        _currentTooltipPathHash = BitGuiContext.Current.PathHash;
        _currentTooltipProvider = GetProvider(control);

        _showTooltipTimer.Start();
    }


    public void EndHover(BitControl control)
    {
        if ((_currentTooltipControl != control) || (_currentTooltipPathHash != BitGuiContext.Current.PathHash))
        {
            return;
        }

        if (_currentTooltipControl != null)
        {
            _currentTooltipControl = null;
        }

        if (_showTooltipTimer != null)
        {
            _showTooltipTimer.Stop();
        }

        if (control != null)
        {
            HideTooltip(control);
        }
    }


    private void OnShowTimer(object sender, ElapsedEventArgs e)
    {
        _showTooltipTimer.Stop();

        if (_currentTooltipControl != null)
        {
            _currentTooltipControl.ShowTooltip = true;
        }
    }


    private void OnHideTimer(object sender, ElapsedEventArgs e)
    {
        _hideTooltipTimer.Stop();
        HideTooltip(_currentTooltipControl);
    }


    private TooltipProvider GetProvider(BitControl control)
    {
        if (control == null)
        {
            return null;
        }

        string providerName = control.TooltipProviderName;

        if (string.IsNullOrEmpty(providerName))
        {
            if (!string.IsNullOrEmpty(control.Content.tooltip))
            {
                // Try to get the simple text tooltip.
                providerName = TextTooltipProviderName;
            }
            else
            {
                return null;
            }
        }

        foreach (TooltipProvider provider in Providers)
        {
            if (providerName.Equals(provider.ProviderName()))
            {
                return provider;
            }
        }

        return null;
    }
}
