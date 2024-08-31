using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;

public class LevelCallbacks : MonoBehaviour
{
    public static event Action onLevelSetup, onLevelRemoved;
    public static AnimatableChildren currentLevelAnimatable;
    
    private void OnEnable()
    {
        currentLevelAnimatable = GetComponent<AnimatableChildren>();
        currentLevelAnimatable.clips[1].events.OnEnded.AddListener(()=>onLevelSetup?.Invoke());
        currentLevelAnimatable.clips[0].events.OnEnded.AddListener(()=>onLevelRemoved?.Invoke());
    }
}
