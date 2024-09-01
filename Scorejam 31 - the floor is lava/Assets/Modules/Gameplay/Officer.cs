using System;
using System.Collections;
using System.Collections.Generic;
using elZach.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Officer : MonoBehaviour
{
    public GameObject projectilePrefab;

    public float shotCooldown = 3f;

    public Vector2 shotOffset = new Vector2(0.3f, 1f);
    private float currentCooldown = 0f;

    private bool shooting = false;
    
    private void OnEnable()
    {
        currentCooldown = Random.Range(0f, shotCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isPlaying || shooting) return;
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0)
        {
            StartCoroutine(Casting());
            currentCooldown = shotCooldown;
        }
    }

    IEnumerator Casting()
    {
        var anim = GetComponentInChildren<SpriteAnimator>();
        if(anim) anim.Play("Cast");
        shooting = true;
        yield return new WaitForSeconds(0.6f);
        if(anim) anim.Play("Idle");
        Shoot();
        shooting = false;
    }

    private void Shoot()
    {
        var dir = GameManager.playerPosition - transform.position;
        dir = dir.XZDirection();
        
        var bullet = projectilePrefab.Spawn();
        SceneManager.MoveGameObjectToScene(bullet.gameObject, this.gameObject.scene);
        bullet.transform.position = transform.position + Vector3.up * shotOffset.y + dir * shotOffset.x;
        bullet.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
