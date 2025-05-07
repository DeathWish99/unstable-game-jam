using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class PlayerController : Entity
{
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * (speed * Time.deltaTime);
        Shoot();
    }

    protected override void Shoot()
    {
        bulletSpawnTimer -= Time.deltaTime;
        
        if (Input.GetKey(KeyCode.Z))
        {
            if (bulletSpawnTimer > 0) return;

            bulletSpawnTimer = bulletSpawnInterval;
        
            foreach (var spawner in bulletSpawners)
            {
                spawner.ShootFromLocalPool();
            }
        }
    }
}
