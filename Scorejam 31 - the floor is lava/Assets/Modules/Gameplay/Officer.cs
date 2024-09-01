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
    
    private void OnEnable()
    {
        currentCooldown = Random.Range(0f, shotCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isPlaying) return;
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0)
        {
            Shoot();
            currentCooldown = shotCooldown;
        }
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
