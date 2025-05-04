using System.Collections.Generic;
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