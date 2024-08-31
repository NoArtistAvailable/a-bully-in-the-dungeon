using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
    private bool activated = false;
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
    }
}
