using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Level
{
    public class EnemyLoader : MonoBehaviour
    {
        public List<GameObject> enemies;
        public bool isAllEnemiesDestroyed = false;
        private void Awake()
        {
            enemies.AddRange(this.GetComponentsInChildren<Entity>().Select(x => x.gameObject).ToList());
        }

        private void Update()
        {
            if (enemies.All(x => x.IsDestroyed()))
            {
                isAllEnemiesDestroyed = true;
            }
        }
    }
}