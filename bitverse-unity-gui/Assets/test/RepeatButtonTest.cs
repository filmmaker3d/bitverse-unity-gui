using Bitverse.Unity.Gui;
using UnityEngine;
using System.Collections;

public class RepeatButtonTest : MonoBehaviour {
    private BitRepeatButton _rb1;
    private BitRepeatButton _rb2;
    private BitRepeatButton _rb3;
    private BitRepeatButton _rb4;

    // Use this for initialization
	void Start () {

        Component[] windows = gameObject.GetComponents(typeof(BitWindow));
        BitWindow window = null;

        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].name == "test_window")
            {
                window = (BitWindow)windows[i];
            }
        }

        if (window == null)
        {
            Debug.LogError("Main window not found.");
            return;
        }


        _rb1 = window.FindControl<BitRepeatButton>("RepeatButton1");
        _rb2 = window.FindControl<BitRepeatButton>("RepeatButton2");
        _rb3 = window.FindControl<BitRepeatButton>("RepeatButton3");
        _rb4 = window.FindControl<BitRepeatButton>("RepeatButton4");

        _rb1.MouseHold += _rb1_MouseHold;
        _rb2.MouseHold += _rb2_MouseHold;
        _rb3.MouseHold += _rb3_MouseHold;
        _rb4.MouseHold += _rb4_MouseHold;
	}

    private void _rb1_MouseHold(object sender, MouseEventArgs e)
    {
        Debug.LogWarning("_rb1_MouseHold");
    }
    
    private void _rb2_MouseHold(object sender, MouseEventArgs e)
    {
        Debug.LogWarning("_rb2_MouseHold");
    }

    private void _rb3_MouseHold(object sender, MouseEventArgs e)
    {
        Debug.LogWarning("_rb3_MouseHold");
    }

    private void _rb4_MouseHold(object sender, MouseEventArgs e)
    {
        Debug.LogWarning("_rb4_MouseHold");
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
