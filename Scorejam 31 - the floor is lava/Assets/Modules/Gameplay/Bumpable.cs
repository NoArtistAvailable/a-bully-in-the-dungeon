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

    private int baseScore = 51;
    private int penalty = 1;

    public List<AudioClip> bumpSfx;
    public List<AudioClip> dieSfx;
    [Vector2Range(0.1f, 2f)] public Vector2 voiceRange = Vector2.one;

    [SerializeField]
    private Button<Bumpable> testAudioButton = new Button<Bumpable>(x => x.PlayVoice(x.bumpSfx.GetRandom()));

    public bool activated { get; private set; } = false;

    public static List<string> nameList = new List<string>() { "Herschel", "Lucas", "Francis", "Conrad", "Heiner", "Kelvin", "Seymore", "Josh", "Alfred", "Pierce", "Paul", "Rennard", "Eve", "Lip", "Rupert", "Gideon", "Milton", "Thaddeus", "Emmett", "Ellis", "Cyrus", "Roland", "Quentin", "Felix", "Silas", "Leopold", "Oscar", "Wallace", "Victor", "Alaric", "Bertram", "Cedric", "Dorian", "Elliot", "Harvey", "Magnus", "Reginald", "Stuart", "Winston", "Ambrose", "Clive", "Ferdinand", "Nigel", "Benedict"};
    
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        gameObject.name = nameList.GetRandom();
        GetComponentInChildren<TextMeshPro>().text = gameObject.name;
        GameManager.onLevelStart += EnablePhysics;
        GameManager.onLevelEnd += DisablePhysics;
    }

    private void OnDisable()
    {
        GameManager.onLevelStart -= EnablePhysics;
        GameManager.onLevelEnd -= DisablePhysics;
    }

    private void EnablePhysics()
    {
        rb.isKinematic = false;
        activated = false;
    }

    private void DisablePhysics()
    {
        rb.isKinematic = true;
        activated = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (Time.fixedTime - lastBump < cooldown) return;
        if (col.body && col.body.CompareTag("Player"))
        {
            lastBump = Time.fixedTime;
            
            var dir = rb.position - GameManager.playerPosition;
            dir.y = 0;
            dir = dir.normalized * bumpPower;
            dir.y = 4f;
            rb.AddForce(dir, ForceMode.VelocityChange);
            baseScore -= penalty;
            PlayVoice(bumpSfx.GetRandom());
        }
    }

    private void FixedUpdate()
    {
        if (activated || rb.isKinematic) return;
        if (rb.position.y < LavaFloor.Instance.transform.position.y)
        {
            activated = true;
            ScoreManager.AddScore($"Killed {gameObject.name}", baseScore, transform.position + Vector3.up * 1.5f);
            PlayVoice(dieSfx.GetRandom());
            VFXManager.SpawnX(rb.position);
        }
    }

    public void PlayVoice(AudioClip clip)
    {
        var audio = GetComponent<AudioSource>();
        audio.pitch = voiceRange.GetRandom();
        audio.PlayOneShot(clip);
    }
}
