using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool fadeIn;
    public bool fadeOut;

    public float fadeSpeed;

    private void Update()
    {
        if (fadeIn)
        {
            if (!(canvasGroup.alpha < 1)) return;
            canvasGroup.blocksRaycasts = true;
            if(Time.timeScale == 0) canvasGroup.alpha += fadeSpeed * Time.fixedUnscaledDeltaTime;
            else canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                fadeIn = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        if (!fadeOut) return;
        if (!(canvasGroup.alpha >= 0)) return;
        canvasGroup.blocksRaycasts = true;
        if(Time.timeScale == 0) canvasGroup.alpha -= fadeSpeed * Time.fixedUnscaledDeltaTime;
        else canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
        if (canvasGroup.alpha <= 0.01f)
        {
            fadeOut = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }
}
