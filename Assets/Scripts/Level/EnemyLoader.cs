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
        private List<GameObject> enemies = new List<GameObject>();
        [HideInInspector]
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