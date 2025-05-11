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
        entityType = EntityType.Player;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        var ver = Input.GetAxis("Vertical");
        var hor = Input.GetAxis("Horizontal");
        var direction = new Vector2(hor, ver).normalized;
        direction *= speed * Time.deltaTime; // apply speed

        var xValidPosition = Mathf.Clamp(transform.position.x + direction.x, xMin, xMax);
        var yValidPosition = Mathf.Clamp(transform.position.y + direction.y, yMin, yMax);

        transform.position = new Vector3(xValidPosition, yValidPosition, 0f);
        Shoot();
    }

    protected override void Shoot()
    {
        bulletSpawnTimer -= Time.deltaTime;
        
        if (Input.GetKey(KeyCode.Z))
        {
            if (bulletSpawnTimer > 0) return;

            bulletSpawnTimer = bulletSpawnInterval;
        
            foreach (var spawner in weaponSpawners)
            {
                spawner.ShootWeaponFromSpawner(entityType);
            }
        }
    }
}
