using UnityEngine;


internal delegate void ShortcutDelegate(Shortcut s);


public class Shortcut
{
	internal readonly EventType _type;
	internal readonly KeyCode _code;
	internal readonly ShortcutDelegate _callback;

	internal Shortcut(EventType type, KeyCode code, ShortcutDelegate callback)
	{
		_code = code;
		_type = type;
		_callback = callback;
	}
}