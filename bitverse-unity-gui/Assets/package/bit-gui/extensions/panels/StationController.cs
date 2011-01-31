using UnityEngine;


[RequireComponent(typeof(BitWindow))]
public class StationController : MonoBehaviour
{
    private BitWindow _window;

    private BitVerticalGroup _topGroup;
    private string _topGroupName = "station_window";

    private BitGroup _bottomGroup;
    private string _bottomGroupName = "undock_window";

    private bool _canUpdate;

    private InvokeUtils.VoidCall startCall;
    void Start() { if (startCall == null) startCall = SafeStart; InvokeUtils.SafeCall(this, startCall); }
    void SafeStart()
    {
        _canUpdate = false;
        _window = GetComponent<BitWindow>();

        if (_window == null)
            return;

        _bottomGroup = _window.FindControl<BitGroup>(_bottomGroupName);
        _topGroup = _window.FindControl<BitVerticalGroup>(_topGroupName);

        if (_bottomGroup == null || _topGroup == null)
            return;

        _canUpdate = true;

    }

    private InvokeUtils.VoidCall updateCall;
    void Update() { if (updateCall == null) updateCall = SafeUpdate; InvokeUtils.SafeCall(this, updateCall); }
    void SafeUpdate()
    {
        if (_canUpdate)
            _window.Enabled = (_bottomGroup.LastFrameWasHover || _topGroup.LastFrameWasHover);
    }


}
