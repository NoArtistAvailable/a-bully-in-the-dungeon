using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [System.Serializable]
    public class Clip
    {
        public string name = "clip";
        public Sprite[] sprites;
        public float duration = 0.1f;
    }

    public List<Clip> clips;
    [NonSerialized] new public SpriteRenderer renderer;
    private Clip _current;

    public Clip current
    {
        get => _current;
        set
        {
            _current = value;
            startTime = Time.time;
        }
    }
    private float startTime;

    public void Play(int index) => this.current = clips[index];
    public void Play(string name) => this.current = clips[clips.FindIndex(x=>x.name == name)];

    void Start()
    {
        Play(0);
    }
    
    void Update()
    {
        if (!renderer) renderer = GetComponentInChildren<SpriteRenderer>();
        if (!renderer || current == null) return;
        float time = Time.time - startTime;
        int index = Mathf.FloorToInt(time / current.duration) % current.sprites.Length;
        renderer.sprite = current.sprites[index];
    }

}
