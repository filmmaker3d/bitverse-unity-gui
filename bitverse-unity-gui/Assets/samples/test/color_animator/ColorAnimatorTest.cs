using UnityEngine;
using System.Collections;

public class ColorAnimatorTest : MonoBehaviour {

	void Start () {

        // ...............................................
        // Coroutine adaptor initialization
        CoRoutineUtils.StartCoroutine = StartCoroutine;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        CoRoutineUtils.WaitForFixedUpdate = delegate() { return wait; };
        CoRoutineUtils.WaitForSeconds = delegate(double delay) { return new WaitForSeconds((float)delay); };

        Component[] windows = gameObject.GetComponents(typeof(BitWindow));
        BitWindow window = null;

        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].name == "coloranimator_window")
            {
                window = (BitWindow)windows[i];
                break;
            }
        }

        if (window == null)
        {
            Debug.LogError("Main window not found.");
            return;
        }

        Debug.Log("Main window loaded. =)");

        BitHorizontalProgressBar animatorHorizontalprogressbar = window.FindControl<BitHorizontalProgressBar>("animator_horizontalprogressbar");

        CoRoutineUtils.StartCoroutine(animatorHorizontalprogressbar.AnimateProgress(animatorHorizontalprogressbar.MinValue, animatorHorizontalprogressbar.MaxValue, 5));
	}
	
	void Update () {
	
	}
}
