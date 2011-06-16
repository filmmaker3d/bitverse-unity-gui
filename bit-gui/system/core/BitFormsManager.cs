using System;
using System.Collections.Generic;
using UnityEngine;


public static class BitFormsManager
{
	#region Form

	private static Dictionary<Guid, BitStage> _stageList = new Dictionary<Guid, BitStage>();

	public static BitStage LoadStage(GameObject gameObject, Type formType)
	{
		if (gameObject == null)
		{
			throw new ApplicationException("GameObject cannot be null");
		}

		BitStage stage = (BitStage) gameObject.AddComponent(formType);

		_stageList.Add(stage.ID, stage);

		//stage.DefaultSkin = defaultSkin;

		//stage.Initialize();

		//stage.OnLoad();

		return stage;
	}

	public static void CloseStage(BitStage stage)
	{
		//stage.OnClose();
		_stageList.Remove(stage.ID);
	}

	#endregion


	#region Modal

	private static Stack<Guid> _stageStack = new Stack<Guid>();

	internal static void PushModal(BitStage source)
	{
		if (_stageStack.Count == 0)
		{
			foreach (KeyValuePair<Guid, BitStage> item in _stageList)
			{
				if (source != item.Value)
				{
					item.Value.Disabled = true;
				}
			}
		}
		else
		{
			_stageList[_stageStack.Peek()].Disabled = true;
		}

		_stageStack.Push(source.ID);

		//Debug.Log(string.Format("Push({0}) '{1}'", _stageStack.Count, source.ID));
	}

	internal static void PopModal()
	{
		if (_stageStack.Count == 1)
		{
			_stageStack.Pop();

			foreach (KeyValuePair<Guid, BitStage> item in _stageList)
			{
				item.Value.Disabled = false;
			}
		}
		else
		{
			_stageStack.Pop();
			_stageList[_stageStack.Peek()].Disabled = false;
		}

		//Debug.Log(string.Format("Pop({0})", _stageStack.Count));
	}

	#endregion
}