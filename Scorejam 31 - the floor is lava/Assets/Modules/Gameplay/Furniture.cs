using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    private bool active = false;
    private void OnEnable()
    {
        GameManager.onLevelStart += EnablePhysics;
        GameManager.onLevelEnd += DisablePhysics;
    }

    private void OnDisable()
    {
        GameManager.onLevelStart -= EnablePhysics;
        GameManager.onLevelEnd -= DisablePhysics;
    }

    private void EnablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        active = true;
    }
    private void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        active = false;
    }

    private void FixedUpdate()
    {
        if (!active) return;
        if (transform.position.y < LavaFloor.Instance.transform.position.y)
        {
            ScoreManager.AddScore("Furniture Destroyed", 3, transform.position + Vector3.up * 1.5f);
            active = false;
        }
    }
}
