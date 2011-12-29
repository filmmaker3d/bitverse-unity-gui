using System.Collections.Generic;
using System.Timers;
using bitgui;
using Bitverse.Unity.Gui;
using UnityEngine;


public class TooltipManager : MonoBehaviour
{
    public string DefaultTooltipProviderName;

    public double ShowTooltipDelay = 500.0;

    public readonly List<TooltipProvider> Providers = new List<TooltipProvider>();

    private BitControl _currentTooltipControl;

    private Timer _showTooltipTimer;

    private int _currentTooltipPathHash;

    private TooltipProvider _currentTooltipProvider;

    private object _currentTooltipData;


    public void Start()
    {
        InvokeUtils.SafeCall(this, SafeStart);
    }


    private void SafeStart()
    {
        _showTooltipTimer = new Timer(ShowTooltipDelay);
        _showTooltipTimer.Elapsed += OnShowTimer;
        _showTooltipTimer.Enabled = false;
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
            ClearControl();

            return true;
        }

        return false;
    }


    public void BeginHover(BitControl control)
    {
        //XXX Do not consider this BeginHover if the control is the same (for lists where this control is not the renderer).
        if ((BitGuiContext.Current.Data != null)
            && (control != null) && (!(control.Parent is AbstractBitList))
            && (_currentTooltipControl == control))
        {
            return;
        }

        EndHover(_currentTooltipControl);

        SetControl(control);

        _showTooltipTimer.Start();
    }


    public void EndHover(BitControl control)
    {
        if (((control != null) && (_currentTooltipControl != control))
            || (_currentTooltipPathHash != BitGuiContext.Current.PathHash))
        {
            return;
        }

        if (_showTooltipTimer != null)
        {
            _showTooltipTimer.Stop();
        }

        if (control != null)
        {
            HideTooltip(control);
        }

        ClearControl();
    }


    private void OnShowTimer(object sender, ElapsedEventArgs e)
    {
        _showTooltipTimer.Stop();

        if (_currentTooltipControl != null)
        {
            _currentTooltipControl.ShowTooltip = true;
            _currentTooltipControl.TooltipData = _currentTooltipData;
        }
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
            if ((!string.IsNullOrEmpty(control.Content.tooltip)) && (!string.IsNullOrEmpty(DefaultTooltipProviderName)))
            {
                // Try to get the default tooltip provider (simple text tooltip, for instance).
                providerName = DefaultTooltipProviderName;
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


    private void SetControl(BitControl control)
    {
        _currentTooltipControl = control;
        _currentTooltipPathHash = BitGuiContext.Current.PathHash;
        _currentTooltipData = BitGuiContext.Current.Data;
        _currentTooltipProvider = GetProvider(control);
    }


    private void ClearControl()
    {
        if (_currentTooltipControl != null)
        {
            _currentTooltipControl.TooltipData = null;
        }

        _currentTooltipControl = null;
        _currentTooltipPathHash = 0;
        _currentTooltipData = null;
        _currentTooltipProvider = null;
    }
}
