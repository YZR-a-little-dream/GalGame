using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadeInEffect : MonoBehaviour
{
    public Image target;
    public float fadeDuration = 2.0f;
    public bool FadeIn = true;
    private void Start()
    {
        SetAlpha(FadeIn);
    }

    private void SetAlpha(bool isFadeIn)
    {
        target.DOFade(isFadeIn ? 0 : 1, fadeDuration);
    }

    private void OnDisable()
    {
        target.DOKill();
    }
}
