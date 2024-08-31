using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMobileInput : MonoBehaviour
{
	public GameObject mobileControls;
	
	public static bool isWebGLOnMobile => Application.isMobilePlatform && Application.platform == RuntimePlatform.WebGLPlayer;
	
    private void OnEnable()
    {
        // bool isWebGLOnDesktop = !Application.isMobilePlatform
        //                         && Application.platform == RuntimePlatform.WebGLPlayer;
        mobileControls.SetActive(isWebGLOnMobile);
    }
}
