using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    public Sprite[] sprites;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sprite = sprites.GetRandom();
    }
}
