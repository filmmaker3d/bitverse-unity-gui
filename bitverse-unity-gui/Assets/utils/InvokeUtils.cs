using System;
using UnityEngine;

public static class InvokeUtils
{
    private const string LogTag = "Invoker";

    public delegate void VoidCall();
    public delegate T ReturnCall<T>();
    public delegate void ParameterCall<T>(T obj);

    public static void SafeCall(object calledClass, VoidCall call)
    {
        try
        {
            call();
        }
        catch (Exception e)
        {
            if (calledClass != null && call != null)
            {
                Debug.Log(string.Format("{0} - Exception while calling {1}\n{2}", calledClass.GetType().Name, call.Method.Name, e));
            }
            else
            {
                Debug.Log(string.Format("{0}Exception calling {1} on {2}\n{3}", LogTag, call, calledClass, e));
            }
        }
    }

    public static void SafeParameter<T>(object calledClass, ParameterCall<T> call, T obj)
    {
        try
        {
            call(obj);
        }
        catch (Exception e)
        {
            if (calledClass != null && call != null)
            {
                Debug.Log(string.Format("{0} - Exception while calling {1}\n{2}", calledClass.GetType().Name, call.Method.Name, e));
            }
            else
            {
                Debug.Log(string.Format("{0}Exception calling {1} on {2}\n{3}", LogTag, call, calledClass, e));
            }
        }
    }

    public static T SafeReturn<T>(object calledClass, ReturnCall<T> call)
    {
        try
        {
            return call();
        }
        catch (Exception e)
        {
            if (calledClass != null && call != null)
            {
                Debug.Log(string.Format("{0} - Exception while calling {1}\n{2}", calledClass.GetType().Name, call.Method.Name, e));
            }
            else
            {
                Debug.Log(string.Format("{0}Exception calling {1} on {2}\n{3}", LogTag, call, calledClass, e));
            }
        }
        return default(T);
    }
}
