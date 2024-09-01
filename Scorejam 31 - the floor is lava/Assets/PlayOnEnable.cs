using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<AudioSource>()?.Play();
    }
}
