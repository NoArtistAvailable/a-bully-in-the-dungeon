using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float speed = 1;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, speed * 360f * Time.deltaTime, Space.Self);
    }
}
