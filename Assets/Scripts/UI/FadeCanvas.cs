using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class FadeCanvas : MonoBehaviour
{
    [Header("事件监听")]
    public FadeEventSO fadeEvent;
    public Image fadeImage;

    private void OnEnable() {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable() {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }
    private void OnFadeEvent(Color color, float duration,bool fadein)
    {
        fadeImage.DOBlendableColor(color, duration);
    }
}
