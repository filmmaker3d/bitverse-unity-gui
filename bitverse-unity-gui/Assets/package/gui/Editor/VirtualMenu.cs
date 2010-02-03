using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public delegate void VirtualButtonDelegate();


public class VirtualMenu
{
	private readonly List<VirtualButton> _buttonList = new List<VirtualButton>();


	public void Add(VirtualButton button)
	{
		_buttonList.Add(button);
	}

	public void ShowMenu(float x, float y)
	{
		foreach (VirtualButton virtualButton in _buttonList)
		{
			virtualButton.Draw(x, y);
			y = y + virtualButton.Height + 20;
		}
	}


	public abstract class VirtualButton
	{
		protected VirtualButton(string label, float width, float height, VirtualButtonDelegate callback)
		{
			Label = label;
			Width = width;
			Height = height;
			Callback = callback;
		}

		private float _width;
		private float _height;
		private string _label;
		private VirtualButtonDelegate _callback;

		public float Height
		{
			get { return _height; }
			set { _height = value; }
		}

		public VirtualButtonDelegate Callback
		{
			get { return _callback; }
			set { _callback = value; }
		}

		public string Label
		{
			get { return _label; }
			set { _label = value; }
		}

		public float Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public void Draw(float x, float y)
		{
			Vector3[] vecs = {
				new Vector3(x, 0, y),
				new Vector3(x + _width, 0, y),
				new Vector3(x + _width, 0, y + _height),
				new Vector3(x, 0, y + _height), new Vector3(x, 0, y)
			};
			Rect rect = new Rect(x, y, _width, _height);
			Vector3 mousepos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
			if (rect.Contains(new Vector2(mousepos.x, mousepos.z))) //translate position
			{
				Handles.color = Color.green;
				if (Event.current.type == EventType.mouseDown)
					_callback.Invoke();
			}
			else
			{
				Handles.color = Color.yellow;
			}

			Handles.DrawPolyLine(vecs);
			Handles.Label(new Vector3(x + 2, 0, y + 2), _label);
			Handles.color = Color.white;
		}
	}
}