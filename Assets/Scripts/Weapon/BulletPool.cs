using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Weapon
{
    public class BulletPool : MonoBehaviour
    {
        public int maxPoolSize = 1000;
        public GameObject bulletPrefab;
        
        private HashSet<GameObject> bullets = new HashSet<GameObject>();
        private void Awake()
        {
            InstantiateBulletsInPool();
        }

        private void Start()
        {
            
        }

        public GameObject GetLastBulletAndRemove()
        {
            var bullet = bullets.Last();
            bullets.Remove(bullet);
            return bullet;
        }

        public void AddBulletsToPool(HashSet<GameObject> bulletsParam)
        {
            bullets.AddRange(bulletsParam);
        }
        
        private void InstantiateBulletsInPool()
        {
            for (int i = 0; i < maxPoolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform);
                
                bullet.SetActive(false);
                bullets.Add(bullet);
            }
        }
    }
}