using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Weapon
{
    public class BulletPool : MonoBehaviour
    {
        public int maxPoolSize = 1000;
        public GameObject enemyBulletPrefab;
        public GameObject playerBulletPrefab;
        
        private HashSet<GameObject> enemyBullets = new HashSet<GameObject>();
        private HashSet<GameObject> playerBullets = new HashSet<GameObject>();
        private void Awake()
        {
            InstantiateBulletsInPool();
        }

        private void Start()
        {
            
        }

        public GameObject GetLastBulletAndRemove(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Player:
                    var bullet = playerBullets.Last();
                    playerBullets.Remove(bullet);
                    return bullet;
                case EntityType.Enemy:
                    var bullet2 = enemyBullets.Last();
                    enemyBullets.Remove(bullet2);
                    return bullet2;
                default:
                    return null;
            }
        }
        public void AddBulletsToPool(HashSet<GameObject> bulletsParam, EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Player:
                    playerBullets.AddRange(bulletsParam);
                    break;
                case EntityType.Enemy:
                    enemyBullets.AddRange(bulletsParam);
                    break;
            }
        }
        
        private void InstantiateBulletsInPool()
        {
            for (int i = 0; i < maxPoolSize; i++)
            {
                GameObject bullet = Instantiate(enemyBulletPrefab, transform);
                GameObject bullet2 = Instantiate(playerBulletPrefab, transform);
                
                bullet.SetActive(false);
                enemyBullets.Add(bullet);
                bullet2.SetActive(false);
                playerBullets.Add(bullet2);
            }
        }
    }
}