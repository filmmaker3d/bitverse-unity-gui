using System;
using System.Collections;
using UnityEngine;


public static class AnimationFun
{

    public static void Animate(IEnumerator animation)
    {
        CoRoutineUtils.StartCoroutine(animation);
    }

    public static IEnumerator Sequential(params IEnumerator[] animations)
    {
        foreach (IEnumerator animation in animations)
            yield return CoRoutineUtils.StartCoroutine(animation);

        yield break;
    }

    public static IEnumerator Parallel(params IEnumerator[] animations)
    {
        foreach (IEnumerator animation in animations)
            CoRoutineUtils.StartCoroutine(animation);

        yield break;
    }

    public static void Animate(Action<float> anim, float duration)
    {
        Animate(anim, duration, null, null);
    }

    public static void Animate(Action<float> anim, float duration, Action<float> executeOnEnd)
    {
        Animate(anim, duration, null, executeOnEnd);
    }

    public static void Animate(Action<float> anim, float duration, Action<float> executeOnStart, Action<float> executeOnEnd)
    {
        CoRoutineUtils.StartCoroutine(Animation(anim, duration, executeOnStart, executeOnEnd));
    }

    public static IEnumerator Animation(Action<float> anim, float duration, Action<float> executeOnStart, Action<float> executeOnEnd)
    {
        if (executeOnStart != null)
            executeOnStart(duration);
        float currentTime = 0;
        anim(0f);
        while (currentTime < (duration - float.Epsilon))
        {
            anim(currentTime / duration);
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
        }
        anim(1.0f);
        if (executeOnEnd != null)
            executeOnEnd(duration);
    }

    public static IEnumerator Animation(Action<float> anim, float duration)
    {
        return Animation(anim, duration, null, null);
    }

    public static IEnumerator Animation(Action<float> anim, float duration, Action<float> executeOnEnd)
    {
        return Animation(anim, duration, null, executeOnEnd);
    }

}