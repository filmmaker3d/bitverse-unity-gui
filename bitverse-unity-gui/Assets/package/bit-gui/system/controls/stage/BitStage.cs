using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEngine;

[RequireComponent(typeof(ContentBasedTextureCache))]

[ExecuteInEditMode]
public class BitStage : BitContainer
{
    public ContentBasedTextureCache TextureCache
    {
        get { return GetComponent<ContentBasedTextureCache>(); }
    }

    public delegate void BeforeOnGUIEventHandler();


    public static event BeforeOnGUIEventHandler BeforeOnGUI;

    private Dictionary<BitWindow, WindowReg> _windowDic = new Dictionary<BitWindow, WindowReg>();
    private WindowReg[] _windows = new WindowReg[0];
    private bool _reversed;
    private BitWindow _hoverWindow;
    private BitWindow _focusedWindow;

    public WindowReg[] GetWindows()
    {
        WindowReg[] ret = new WindowReg[0];
        Array.Resize(ref ret, _windows.Length);
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = _windows[_reversed ? ret.Length - i - 1 : i];
        }
        return ret;
    }

    //public bool drawWindows = true;

    public BitWindow HoverWindow
    {
        get { return _hoverWindow; }
        set { _hoverWindow = value; }
    }

    public BitWindow FocusedWindow
    {
        get { return _focusedWindow; }
        set { _focusedWindow = value; }
    }

    public bool IsHoverAnyActiveControl
    {
        get { return _hoverWindow != null; }
    }

    protected virtual void DoBeforeOnGUI()
    {
        if (BeforeOnGUI != null)
        {
            BeforeOnGUI();
        }
    }

    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();

        _lastCursorTime = Time.time;
        WindowReg.FormMode2SortIndex[FormModes.Popup] = 1;
        WindowReg.FormMode2SortIndex[FormModes.Messages] = 2;
        WindowReg.FormMode2SortIndex[FormModes.Modal] = 3;
        WindowReg.FormMode2SortIndex[FormModes.Loading] = 4;
        WindowReg.FormMode2SortIndex[FormModes.Panel] = 5;
        WindowReg.FormMode2SortIndex[FormModes.Normal] = 6;
        WindowReg.FormMode2SortIndex[FormModes.Background] = 7;

        if (_dragManager == null)
        {
            _dragManager = new BitDragManager(this);
        }

    }

    private int _errorCountDrawNonWindow;
    public bool hideAll = false;

    public virtual void OnGUI()
    {
        if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.F11 && Event.current.control)
        {
            hideAll = !hideAll;
        }
        if (hideAll)
            return;

        OriginalEvent = Event.current.type;
        ThereIsAFocusedControl = false;
        try
        {
            Current = this;
            if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Ignore)
            {
                foreach (WindowReg reg in _windowDic.Values)
                    reg.Sweep = true;
                if (Position.width != Screen.width || Position.height != Screen.height)
                {
                    Size = new Size(Screen.width, Screen.height);
                    LayoutChildren();
                }

                //stage events should be global...
                RaiseMouseMove(Event.current.mousePosition);

                if (Event.current.type == EventType.MouseUp) //Input.GetMouseButtonUp(MouseButtons.Left))
                {
                    RaiseMouseUp(Event.current.button, Event.current.mousePosition);
                }

                if (Visible)
                {
                    DoBeforeOnGUI();

                    GUI.matrix = transform.localToWorldMatrix;

                    for (int i = transform.childCount - 1; i >= 0; i--)
                    {
                        BitControl c = transform.GetChild(i).GetComponent<BitControl>();
                        if (c is BitWindow)
                        {
                            BitWindow win = (BitWindow)c;
                            if (win.DestroyMe && (c.animation == null || !c.animation.isPlaying))
                            {
                                _windowDic.Remove(win);
                                c.Parent = null;
                                c.transform.parent = null;
                                win._destroyCallback(0);
                                Destroy(c.gameObject);
                            }
                            else
                                RegWindow(win);
                        }
                        else
                        {
                            _errorCountDrawNonWindow++;
                            if (_errorCountDrawNonWindow > 500)
                            {
                                _errorCountDrawNonWindow = 0;
                                Debug.LogError(string.Format("BitStage - Wrong usage, added something other than a window to a bit stage: Name={0}", c.name));
                            }
                        }
                    }
                }
                List<WindowReg> toDelete = null;
                foreach (WindowReg reg in _windowDic.Values)
                    if (reg.Sweep)
                        (toDelete ?? (toDelete = new List<WindowReg>())).Add(reg);
                if (toDelete != null)
                    foreach (WindowReg reg in toDelete)
                        _windowDic.Remove(reg.Window);
            }
            if (_windowDic.Count > 0)
            {
                Array.Resize(ref _windows, _windowDic.Count);
                // inserts the windows after the last empty bucket.
                _windowDic.Values.CopyTo(_windows, 0);

                _reversed = false;
                Array.Sort(_windows);


                WindowReg topModalWindow = null;
                WindowReg topPopupWindow = null;
                WindowReg topMessagesWindow = null;
                WindowReg topLoadingWindow = null;
                WindowReg topPanelWindow = null;
                WindowReg topNormalWindow = null;
                WindowReg topBackgroundWindow = null;

                if (Event.current.type == EventType.Layout)
                    _hoverWindow = null;

                bool leaveLoop = false;
                for (int j = _windows.Length - 1; j >= 0; j--)
                {
                    WindowReg r = (WindowReg)_windows.GetValue(j);

                    if (!r.Visible)
                        break;

                    switch (r.FormMode)
                    {
                        case FormModes.Popup:
                            topPopupWindow = (topPopupWindow ?? r);
                            break;
                        case FormModes.Messages:
                            topMessagesWindow = (topMessagesWindow ?? r);
                            break;
                        case FormModes.Modal:
                            topModalWindow = (topModalWindow ?? r);
                            break;
                        case FormModes.Loading:
                            topLoadingWindow = (topLoadingWindow ?? r);
                            break;
                        case FormModes.Panel:
                            topPanelWindow = (topPanelWindow ?? r);
                            break;
                        case FormModes.Background:
                            topBackgroundWindow = (topBackgroundWindow ?? r);
                            leaveLoop = true; // last window type
                            break;
                        default:
                            topNormalWindow = (topNormalWindow ?? r);
                            break;
                    }
                    if (leaveLoop)
                        break;
                }

                // Mouse handling and other events should be processed top first.
                if (Event.current.type != EventType.Repaint)
                {
                    _reversed = true;
                    Array.Reverse(_windows);
                }

                //WindowReg firstWindowReg = null;
                foreach (WindowReg reg in _windows)
                {
                    try
                    {
                        if (reg.Window == null)
                            continue;

                        bool lastEnabled = reg.Window.Enabled;
                        if (topModalWindow != null && reg.Window != topModalWindow.Window && DisablableByModal(reg.FormMode))
                            reg.Window.Enabled = false;
                        reg.Window.Draw();
                        reg.Window.Enabled = lastEnabled;

                        // Consume Events in modal
                        if (reg.Window.FormMode == FormModes.Modal && Event.current.type != EventType.Repaint && reg.Visible)
                        {
                            Event.current.Use();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(string.Format("BitStage - Errors found during BitStage Draw: WindowName={0}" + Environment.NewLine + ex, reg.WindowName));
                    }
                }
            }
            _isAnyControlFocused = ThereIsAFocusedControl;
            UpdateCursor();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
		if(TextureCache!=null)
			TextureCache.Cleanup();
    }

    private bool DisablableByModal(FormModes mode)
    {
        return mode == FormModes.Panel || mode == FormModes.Normal || mode == FormModes.Modal || mode == FormModes.Background;
    }

    public override void OnDrawGizmos()
    {
        DrawRect(new Color(0f, 1f, 1f, 0.2f), AbsolutePosition);
    }

    #endregion


    #region DragManager

    private BitDragManager _dragManager;

    public BitDragManager DragManager
    {
        get { return _dragManager; }
    }

    #endregion


    public static BitStage Current;


    #region Draw

    protected override void DoDraw()
    {
        //does nothing :)
    }

    #endregion


    private bool _isAnyControlFocused;

    public bool IsAnyControlFocused
    {
        get
        {
            return _isAnyControlFocused;
            //return GUIUtility.keyboardControl != 0;
        }
    }


    #region Audio

    public delegate void AudioEventHandler(BitControl control, BitAudioEventTypeEnum type, string text);


    public static event AudioEventHandler Audio;

    public void RaiseAudio(BitControl control, BitAudioEventTypeEnum type, string text)
    {
        if (Audio != null)
        {
            Audio(control, type, text);
        }
    }

    #endregion

    public class WindowReg : IComparable<WindowReg>
    {
        public BitWindow Window;
        public FormModes FormMode;
        public int FormModeSortIndex;
        public float Depth;
        public bool Sweep;
        public GUISkin Skin;
        public string WindowName;
        public bool Visible;

        #region Implementation of IComparable<WindowReg>

        public int CompareTo(WindowReg other)
        {
            if (other == null)
                return -1;

            if (Visible && !other.Visible)
                return 1;
            if (!Visible && other.Visible)
                return -1;

            if (FormMode != Window.FormMode)
                FormModeSortIndex = FormMode2SortIndex[Window.FormMode];

            if (other.FormMode != other.Window.FormMode)
                other.FormModeSortIndex = FormMode2SortIndex[other.Window.FormMode];

            int diff = other.FormModeSortIndex - FormModeSortIndex;

            if (diff != 0)
                return diff;

            if (Depth < other.Depth)
                return -1;
            if (Depth > other.Depth)
                return 1;
            if (Window == other.Window)
                return 0;
            if (Window == null)
                return -1;
            if (other.Window == null)
                return 1;
            return (Window.name ?? "").CompareTo((other.Window.name ?? ""));
        }

        public static Dictionary<FormModes, int> FormMode2SortIndex = new Dictionary<FormModes, int>();/*
                                                                 {
                                                                     {FormModes.Popup, 1},
                                                                     {FormModes.Messages,2},
                                                                     {FormModes.Modal,3},
                                                                     {FormModes.Loading,4},
                                                                     {FormModes.Panel,5},
                                                                     {FormModes.Normal,6},
                                                                     {FormModes.Background,7}
                                                                 };*/

        #endregion
    }



    public void RegWindow(BitWindow window)
    {
        WindowReg reg;
        if (!_windowDic.TryGetValue(window, out reg))
        {
            reg = new WindowReg();
            reg.Window = window;
            _windowDic.Add(window, reg);
        }

        reg.Skin = GUI.skin;
        reg.Sweep = false;
        reg.FormMode = window.FormMode;
        reg.Depth = window.Depth;
        reg.WindowName = window.name;
        reg.Visible = window.Visible;

        // to speedup comparison
        if(!WindowReg.FormMode2SortIndex.TryGetValue(window.FormMode, out reg.FormModeSortIndex))
        {
            reg.FormModeSortIndex = 6;
        }
    }

    public void BringWindowToFront(BitWindow window)
    {
        float max = 0;
        WindowReg wreg = null;
        foreach (WindowReg reg in _windowDic.Values)
        {
            if (window != reg.Window) max = Mathf.Max(max, reg.Window.Depth);
            else wreg = reg;
        }
        window.Depth = Mathf.Max(max + 1, window.Depth);
        if (wreg != null) wreg.Depth = window.Depth;
        FocusedWindow = window;
    }

    public void SendWindowToBack(BitWindow window)
    {
        float min = 0;
        WindowReg wreg = null;
        foreach (WindowReg reg in _windowDic.Values)
        {
            if (window != reg.Window) min = Mathf.Min(min, reg.Window.Depth);
            else wreg = reg;
        }
        window.Depth = Mathf.Min(min - 1, window.Depth);
        if (wreg != null) wreg.Depth = window.Depth;
        if (FocusedWindow == window)
            FocusedWindow = null;
    }

    #region mousecursor

    public enum CursorState
    {
        INVISIBLE,
        NORMAL,
        OVER,
        DISABLED,
        PROCESSING,
        SYSTEM,
        COMBAT
    }

    public Texture2D[] normalCursor;
    public Vector2 normalOffset;
    public Texture2D[] overCursor;
    public Vector2 overOffset;
    public Texture2D[] disabledCursor;
    public Vector2 disabledOffset;
    public Texture2D[] processingCursor;
    public Vector2 processingOffset;
    public Texture2D[] combatCursor;
    public Vector2 combatOffset;
    private Texture2D _currentCursor;
    private Texture2D[] _currentCursorArray;
    private Vector2 _currentOffset;
    public CursorState currentCursorState;
    public float cursorRate = 0.1f;
    private CursorState _lastCursorState;
    private int cursorIndex = 0;
    private float _lastCursorTime;

    public void RefreshCursor()
    {
        //This will trigger the code inside UpdateCursor()
        _lastCursorState = CursorState.INVISIBLE;
    }

    private void UpdateCursor()
    {
        //if (currentCursorState == null)
        //{
        //    return;
        //}
        if (_lastCursorState != currentCursorState)
        {
            Screen.showCursor = false;
            cursorIndex = 0;
            switch (currentCursorState)
            {
                case CursorState.NORMAL:
                    _currentCursorArray = normalCursor;
                    _currentOffset = normalOffset;
                    break;
                case CursorState.OVER:
                    _currentCursorArray = overCursor;
                    _currentOffset = overOffset;
                    break;
                case CursorState.PROCESSING:
                    _currentCursorArray = processingCursor;
                    _currentOffset = processingOffset;
                    break;
                case CursorState.DISABLED:
                    _currentCursorArray = disabledCursor;
                    _currentOffset = disabledOffset;
                    break;
                case CursorState.COMBAT:
                    _currentCursorArray = combatCursor;
                    _currentOffset = combatOffset;
                    break;
                case CursorState.INVISIBLE:
                    _currentCursorArray = null;
                    break;
                case CursorState.SYSTEM:
                    Screen.showCursor = true;
                    _currentCursorArray = null;
                    break;
            }
            _lastCursorState = currentCursorState;
        }
        if (_currentCursorArray != null && cursorIndex > 0 && cursorIndex<_currentCursorArray.Length)
        {
            Vector3 pos = Input.mousePosition;
            Texture2D tex = _currentCursorArray[cursorIndex];
            GUI.DrawTexture(new Rect(pos.x + _currentOffset.x, Screen.height - pos.y + _currentOffset.y, tex.width, tex.height), tex);
            if ((Time.time - _lastCursorTime) > cursorRate)
            {
                if (cursorIndex == (_currentCursorArray.Length - 1))
                {
                    cursorIndex = 0;
                }
                else
                {
                    cursorIndex++;
                }
                _lastCursorTime = Time.time;
            }
        }
    }

    public void OnDisable()
    {
        Screen.showCursor = true;
    }

    #endregion
}
