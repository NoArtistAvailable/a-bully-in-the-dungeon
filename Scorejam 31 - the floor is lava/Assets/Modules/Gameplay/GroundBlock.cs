using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
    private bool activated = false;
    public List<AudioClip> audio;
    [Vector2Range(0.1f,2f)]public Vector2 audioRange = Vector2.one;

    public event Action onSunk;

    void OnCollisionExit(Collision col)
    {
        if (activated) return;
        if (!col.body.CompareTag("Player")) return;
        var boxCollider = this.GetComponent<BoxCollider>();
        boxCollider.size *= 0.9f;
        var rb = this.gameObject.AddComponent<Rigidbody>();
        rb.drag = 5f;
        activated = true;
        ScoreManager.AddScore("Floor Destroyed", 1); //, transform.position + Vector3.up * 1.5f);
        AudioManager.PlayClip(audio.GetRandom(), audioRange.GetRandom(), 1f);
        onSunk?.Invoke();
    }
}
