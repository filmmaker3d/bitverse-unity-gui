using System;
using System.Collections;


public static class InvokeUtils
{

    private const string LogTag = "Invoker";

    public delegate void VoidCall();
    public delegate T ReturnCall<T>();
    public delegate void ParameterCall<T>(T obj);
    public delegate T1 ParamReturnCall<T1, T2>(T2 obj);

    public static void SafeCall(object calledClass, VoidCall call)
    {
        try
        {

            call();


        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(
                     string.Format("Exception while calling {0} on {1}",
                     (call == null ? "$null$" : call.Method.Name),
                     (calledClass == null ? "$null$" : calledClass.GetType().Name)));
            UnityEngine.Debug.LogError(e);
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

            Type type = typeof(T);
            string value = type.IsValueType ? obj.ToString() : (obj == null ? "$null$" : obj.ToString());
            UnityEngine.Debug.LogError(
                string.Format("Exception while calling {0} on {1} with param: type={2} value={3}",
                              (call == null ? "$null$" : call.Method.Name),
                              (calledClass == null ? "$null$" : calledClass.GetType().Name),
                              type.Name, value));
            UnityEngine.Debug.LogError(e);

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
            UnityEngine.Debug.LogError((
                                           string.Format("Exception while calling return {0} on {1}",
                                                         (call == null ? "$null$" : call.Method.Name),
                                                         (calledClass == null ? "$null$" : calledClass.GetType().Name))));
                    UnityEngine.Debug.LogError(e);
        }
        return default(T);
    }

    public static T1 SafeParamReturn<T1, T2>(object calledClass, ParamReturnCall<T1, T2> call, T2 obj)
    {
        try
        {
            return call(obj);
        }
        catch (Exception e)
        {
            
                Type type = typeof(T2);
                string value = type.IsValueType ? obj.ToString() : (obj == null ? "$null$" : obj.ToString());
                 UnityEngine.Debug.LogError(
                    string.Format("Exception while calling return {0} on {1} with param: type={2} value={3}",
                                  (call == null ? "$null$" : call.Method.Name),
                                  (calledClass == null ? "$null$" : calledClass.GetType().Name),
                                  type.Name, value));
            UnityEngine.Debug.LogError(e);
            
        }
        return default(T1);
    }
}
