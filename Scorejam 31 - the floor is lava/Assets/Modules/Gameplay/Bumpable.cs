using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using TMPro;
using UnityEngine;

public class Bumpable : MonoBehaviour
{
    public static float bumpPower = 2.5f;
    private Rigidbody rb;

    private float cooldown = 0.8f;
    private float lastBump = 0;

    private int baseScore = 55;
    private int penalty = 5;

    private bool activated = false;

    public static List<string> nameList = new List<string>() { "Herschel", "Lucas", "Francis", "Conrad", "Heiner", "Kelvin", "Seymore", "Josh", "Alfred", "Pierce", "Paul", "Rennard", "Eve", "Lip"};
    
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.name = nameList.GetRandom();
        GetComponentInChildren<TextMeshPro>().text = gameObject.name;
    }

    void OnCollisionEnter(Collision col)
    {
        if (Time.fixedTime - lastBump < cooldown) return;
        if (col.body && col.body.CompareTag("Player"))
        {
            lastBump = Time.fixedTime;
            Debug.Log($"{name} got bumped | {col.impulse}", this);
            
            var dir = rb.position - GameManager.playerPosition;
            dir.y = 0;
            dir = dir.normalized * bumpPower;
            dir.y = 4f;
            rb.AddForce(dir, ForceMode.VelocityChange);
            baseScore -= penalty;
        }
    }

    private void FixedUpdate()
    {
        if (activated) return;
        if (rb.position.y < LavaFloor.Instance.transform.position.y)
        {
            activated = true;
            ScoreManager.AddScore($"Killed {gameObject.name}", baseScore);
        }
    }
}
