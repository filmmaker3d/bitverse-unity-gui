using UnityEngine;


public static class BitGUIDebug
{
	public static void DrawRect(Color color, Rect rect)
	{
		Debug.DrawLine(new Vector3(rect.x, 0, rect.y), new Vector3(rect.xMax, 0, rect.y), color);
		Debug.DrawLine(new Vector3(rect.xMax, 0, rect.y), new Vector3(rect.xMax, 0, rect.yMax), color);
		Debug.DrawLine(new Vector3(rect.xMax, 0, rect.yMax), new Vector3(rect.x, 0, rect.yMax), color);
		Debug.DrawLine(new Vector3(rect.x, 0, rect.yMax), new Vector3(rect.x, 0, rect.y), color);
	}
}