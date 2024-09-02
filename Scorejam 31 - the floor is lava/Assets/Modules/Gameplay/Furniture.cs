using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    private bool active = false;
    private Transform child;
    private void OnEnable()
    {
        GameManager.onLevelStart += EnablePhysics;
        GameManager.onLevelEnd += DisablePhysics;
        child = transform.GetChild(0);
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

    private Transform cam;
    private void Update()
    {
        if (!cam) cam = Camera.main.transform;
        var dir = -cam.forward;
        child.rotation = Quaternion.LookRotation(dir, child.up);

    }
}
