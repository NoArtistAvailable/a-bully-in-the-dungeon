using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteSelector : MonoBehaviour
{
    public Sprite[] sprites;
    public bool randomDirection = true;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sprite = sprites.GetRandom();
        var val = transform.localScale;
        if (randomDirection) transform.localScale = new Vector3(Random.value > 0.5f ? val.x : -val.x, val.y, val.z);
    }
}
