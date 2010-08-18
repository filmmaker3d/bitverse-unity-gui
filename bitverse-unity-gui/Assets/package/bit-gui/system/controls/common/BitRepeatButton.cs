using Bitverse.Unity.Gui;
using UnityEngine;


public class BitRepeatButton : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.button; }
	}

	#endregion
    
	#region Data

	private bool _value;

    private bool _mouseIsDown;
    private bool _startRepeat;
    private float _lastTime;

    [SerializeField]
    private float _timeToStartRepeat = 1.0f;

    public float TimeToStartRepeat
    {
        get { return _timeToStartRepeat; }
        set{ _timeToStartRepeat = value; }
        
    }

    [SerializeField]
    private float _timeToRepeat = 0.1f;

    public float TimeToRepeat
    {
        get { return _timeToRepeat; }
        set{ _timeToRepeat = value; }
        
    }


	#endregion
    
	#region Draw

	protected override void DoDraw()
    {
        (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn | ForceOnState, Focus);
	    _value = (IsHover && IsActive);
		//_value = GUI.RepeatButton(Position, Content, Style ?? DefaultStyle);
	}

	#endregion
    
    #region Events

    public event MouseHoldEventHandler MouseHold;

    protected void RaiseMouseHold(int mouseButton, Vector2 mousePosition)
    {
        if (MouseHold != null)
        {
            MouseHold(this, new MouseEventArgs(mouseButton, mousePosition));
        }
    }

    protected override bool UserEventsAfterDraw()
    {
        float currentTime = Time.time;

        if (!_mouseIsDown)
            _lastTime = currentTime;

		if (!_value)
			return false;

        if (!_startRepeat)
        {
            if (currentTime - _lastTime >= TimeToStartRepeat)
            {
                _startRepeat = true;
            }
        }

        if (_startRepeat && currentTime - _lastTime >= TimeToRepeat)
        {
            RaiseMouseHold(Event.current.button, Event.current.mousePosition);
            _lastTime = currentTime;
            return false;
        }

		return true;
	}

    private void ResetVariables()
    {
        _mouseIsDown = false;
        _startRepeat = false;
        _lastTime = Time.time;
    }

    protected override void RaiseMouseDown(int mouseButton, Vector2 mousePosition)
    {
        _mouseIsDown = true;
        base.RaiseMouseDown(mouseButton, mousePosition);
    }
    protected override void RaiseMouseUp(int mouseButton, Vector2 mousePosition)
    {
        ResetVariables();
        base.RaiseMouseUp(mouseButton,mousePosition);
    }

    protected override bool ConsumeEvent(EventType type)
    {
        return true;
    }

    #endregion

    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();
        ResetVariables();
    }

    #endregion

}