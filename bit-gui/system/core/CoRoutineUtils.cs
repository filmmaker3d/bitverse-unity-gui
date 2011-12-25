using System;
using System.Collections;


public static class CoRoutineUtils
{
    public delegate object StartDelegate(IEnumerator task);
    public delegate object FixedUpdateDelegate();
    public delegate object DelayDelegate(double delay);


    public static long Coroutines = 0;

    private static StartDelegate _startCoroutine;
    public static StartDelegate StartCoroutine
    {
        get { return _startCoroutine; }
        set
        {
            _startCoroutine = delegate(IEnumerator task)
            {
                Coroutines++;
                return value(task);
            };
        }
    }

    public static DelayDelegate WaitForSeconds;
    public static FixedUpdateDelegate WaitForFixedUpdate;
}
