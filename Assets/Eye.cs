using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Eye : MonoBehaviour
{

    public ScreenTransitionImageEffect effect;
    public void EyeOpen(float time, bool EnableAndDisable, Action callback = null, float delay = 0)
    {
        StartCoroutine(EO(time, EnableAndDisable, callback, delay));
    }
    IEnumerator EO(float time, bool enable, Action callback, float delay)
    {
		if (delay!=0) yield return new WaitForSeconds(delay);
        if (enable) effect.enabled = true;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            float t = i / time;
            t = t * t * (3f - 2f * t);
            effect.maskValue = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        effect.maskValue = 0;
        if (enable) effect.enabled = false;
        if (callback != null) callback();
    }
    public void EyeClose(float time, bool EnableAndDisable, Action callback = null)
    {
        StartCoroutine(EC(time, EnableAndDisable, callback));
    }
    IEnumerator EC(float time, bool enable, Action callback)
    {
        if (enable) effect.enabled = true;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            float t = i / time;
            t = t * t * (3f - 2f * t);
            effect.maskValue = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        effect.maskValue = 1;
        if (enable) effect.enabled = false;
        if (callback != null) callback();
    }
}
