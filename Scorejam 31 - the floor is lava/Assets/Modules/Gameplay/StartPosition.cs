using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    public static StartPosition Instance;
    private void OnEnable()
    {
        Instance = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.9f, 0f, 0.3f);
        Gizmos.DrawCube(transform.position+Vector3.up * 0.5f, Vector3.one);
    }
}
