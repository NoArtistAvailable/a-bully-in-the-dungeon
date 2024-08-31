using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEditor;
using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    private bool activated = true;
    private float radius = 0.25f;

    private void OnEnable() => LevelCallbacks.onLevelSetup += Ready;
    private void OnDisable() => LevelCallbacks.onLevelSetup -= Ready;
    private void Ready() => activated = false;

    private void FixedUpdate()
    {
        //check player position
        var dist = Vector2.Distance(GameManager.Instance.player.transform.position.xz(), transform.position.xz());
        if(dist <radius) Finish();
    }

    void Finish()
    {
        if (activated) return;
        activated = true;
        // for(int i= 0; i < 10; i++) ScoreManager.AddScore($"Did a thing [{i}]", UnityEngine.Random.Range(100, 400));
        ScoreManager.AddScore("Finished", 100);
        GameManager.Instance.FinishLevel(false);
    }
    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!GameManager.Instance || !GameManager.Instance.player) return;
        Handles.Label(transform.position + Vector3.up, Vector2.Distance(GameManager.Instance.player.transform.position.xz(), transform.position.xz()).ToString());
        Handles.DrawWireDisc(transform.position, Vector3.up, radius);
    }
    #endif
}
