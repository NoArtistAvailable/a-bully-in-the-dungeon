using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 3f;

    private Rigidbody rb;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
    }
}
