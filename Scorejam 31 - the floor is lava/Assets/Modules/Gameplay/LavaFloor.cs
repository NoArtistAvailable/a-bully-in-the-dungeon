using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    private static LavaFloor _instance;
    public static LavaFloor Instance => _instance.OrSet(ref _instance, FindObjectOfType<LavaFloor>);
    
    private bool activated = true;
    private void OnEnable() => LevelCallbacks.onLevelSetup += Ready;
    private void OnDisable() => LevelCallbacks.onLevelSetup -= Ready;
    private void Ready() => activated = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (activated) return;
        if (!GameManager.Instance.player.enabled) return;
        if (GameManager.playerPosition.y < transform.position.y)
        {
            activated = true;
            GameManager.Instance.player.GetComponentInChildren<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
            GameManager.Instance.FinishLevel(true);
        } 
    }
}
