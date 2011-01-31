using Bitverse.Unity.Gui;
using UnityEngine;


public interface IPopup
{
	bool AlwaysInsideScreen { get; set; }

	void Show(Point position, GUISkin skin);

	void Show(Vector2 position, GUISkin skin);

	void Show(Point position);

	void Show(Vector2 position);

	void Hide();
}