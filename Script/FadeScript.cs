using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup MyUI;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut() 
    {
        fadeOut = true;
    }

    public void Update()
    {
        if (fadeIn) 
        {
            if (MyUI.alpha < 1) 
            {
                MyUI.alpha += Time.deltaTime;
                if (MyUI.alpha >= 1) 
                {
                    fadeIn = false;
                }
            }
        }


        if (fadeOut)
        {
            if (MyUI.alpha >= 0)
            {
                MyUI.alpha -= Time.deltaTime;
                if (MyUI.alpha == 0)
                {
                    fadeIn = fadeOut;
                }
            }
        }
    }
}
