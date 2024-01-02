using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class CanvasGroupScreenFade : MonoBehaviour
{
    public void Initialize()
    {

    }

    public void ShowCnavasGroup(CanvasGroup cg, float delayTime, float durationTime, Action onResultCallback)
    {
        StartCoroutine(Show(cg, delayTime, durationTime, onResultCallback));
    }

    public void HideCnavasGroup(CanvasGroup cg, float delayTime, float durationTime, Action onResultCallback)
    {
        StartCoroutine(Hide(cg, delayTime, durationTime, onResultCallback));
    }

    IEnumerator Show(CanvasGroup cg, float delayTime, float durationTime, Action onResultCallback) // change UniTask
    {
        cg.gameObject.SetActive(true);
        cg.alpha = 0;

        yield return new WaitForSeconds(delayTime);

        try
        {
            cg.DOFade(1.0f, durationTime).OnComplete(() => { onResultCallback?.Invoke(); });
        }
        catch
        {

        }
    }

    IEnumerator Hide(CanvasGroup cg, float delayTime, float durationTime, Action onResultCallback) // change UniTask
    {
        cg.alpha = 1;

        yield return new WaitForSeconds(delayTime);

        try
        {
            cg.DOFade(0.0f, durationTime).OnComplete(() =>
            {
                cg.gameObject.SetActive(false);

                onResultCallback?.Invoke();
            });
        }
        catch
        {

        }
    }
}
