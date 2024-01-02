using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UiAnimation : MonoBehaviour
{
    public void Initialize()
    {

    }

    public void Move(RectTransform rTr, Vector2 targetPosition, float fadeTime, bool snapping, Action onResult, Ease easeMode = Ease.Unset)
    {
        if(easeMode == Ease.Unset)
        {
            rTr.DOLocalMove(targetPosition, fadeTime, snapping).OnComplete(() => { onResult?.Invoke(); }); ;
        }
        else
        {
            rTr.DOLocalMove(targetPosition, fadeTime, snapping).SetEase(easeMode).OnComplete(() => { onResult?.Invoke(); });
        }
    }

    public void MoveX(RectTransform rTr, float endValue, float duration, bool snapping, Action onResult)
    {
        rTr.DOAnchorPosX(endValue, duration, snapping).OnComplete(() => { onResult?.Invoke(); });
    }
}
