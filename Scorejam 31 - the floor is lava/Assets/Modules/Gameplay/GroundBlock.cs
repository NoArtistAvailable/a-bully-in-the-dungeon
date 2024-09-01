using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
    private bool activated = false;
    public List<AudioClip> audio;
    [Vector2Range(0.1f,2f)]public Vector2 audioRange = Vector2.one;
    
    void OnCollisionExit(Collision col)
    {
        if (activated) return;
        if (!col.body.CompareTag("Player")) return;
        var boxCollider = this.GetComponent<BoxCollider>();
        boxCollider.size *= 0.9f;
        var rb = this.gameObject.AddComponent<Rigidbody>();
        rb.drag = 5f;
        activated = true;
        ScoreManager.AddScore("Floor Destroyed", 1);
        AudioManager.PlayClip(audio.GetRandom(), audioRange.GetRandom(), 1f);
    }
}
