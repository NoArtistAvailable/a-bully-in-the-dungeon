using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VFXManager : MonoBehaviour
{
    private static VFXManager _instance;
    public static VFXManager Instance => _instance.OrSet(ref _instance, FindObjectOfType<VFXManager>);
    public Transform xSprite;

    private void OnEnable()
    {
        xSprite.gameObject.SetActive(false);
    }

    public static void SpawnX(Vector3 position)
    {
        if (SceneManager.loadedSceneCount < 1) return;
        var additiveScene = SceneManager.GetSceneAt(1);

        var clone = Instance.xSprite.Spawn();
        SceneManager.MoveGameObjectToScene(clone.gameObject, additiveScene);
        clone.position = position;
        clone.gameObject.SetActive(true);
        clone.gameObject.hideFlags = HideFlags.DontSave;
    }
}
