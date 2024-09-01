using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    public float moveSpeed = 1f;

    public float range = 3f;

    private Rigidbody rb;

    private Vector3 startPoint, endPoint;
    private float startTime;
    
    void OnEnable()
    {
        GameManager.onLevelStart += Setup;
    }

    private void OnDisable()
    {
        GameManager.onLevelStart -= Setup;
    }

    void Setup()
    {
        startPoint = transform.position;
        endPoint = transform.position + transform.forward * range;
        startTime = Time.time;
        ScoreManager.AddScore("Obstacle Bonus", 10);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!GameManager.isPlaying) return;

        float distanceTraveled = (Time.time - startTime) * moveSpeed;
        float distanceRelativeToRange = distanceTraveled / range;
        distanceRelativeToRange %= 2f;
        if (distanceRelativeToRange > 1f) distanceRelativeToRange = distanceRelativeToRange.Remap(1f, 2f, 1f, 0f);
        // Debug.Log($"{distanceTraveled} : {distanceRelativeToRange}");
        rb.MovePosition( Vector3.Lerp(startPoint,endPoint, distanceRelativeToRange));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.isPlaying) return;
        if (other.attachedRigidbody && other.attachedRigidbody.CompareTag("Player"))
        {
            GameManager.Instance.player.Kill();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * range);
    }
}
