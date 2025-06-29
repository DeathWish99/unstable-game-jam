﻿using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Weapon
{
    public class BulletSpawner : MonoBehaviour, ISpawner
    {
        public BulletBase.BulletType bulletType;
        public int bulletCount = 100;
        
        [HideInInspector]
        public float angle = 0;
        
        public float bulletLife = 1f;
        public float speed = 1f;
        
        //Zig Zag
        [HideInInspector]
        public float magnitude = 0f; //Size of sine movement
        [HideInInspector]
        public float frequency = 0f; //Speed of sine movement

        //Curved
        [HideInInspector]
        [Tooltip("Controls the direction/speed the spiral translates")]
        public Vector2 linearVelocity = new Vector2(0.5f, 0f);
    
        [HideInInspector]
        [Tooltip("Controls the radius of the circular motion for X/Y axis.")]
        public Vector2 radii = new Vector2(2f, 3f);
        public bool isTypeZigZagOrCurved => bulletType == BulletBase.BulletType.ZigZag || bulletType == BulletBase.BulletType.Curved;

        private HashSet<GameObject> bullets = new HashSet<GameObject>();
        private GameObject loadedBullet
        {
            get => bullets.LastOrDefault(x => x.activeSelf == false);
        }
        
        private BulletBase loadedBulletComponent
        {
            get => loadedBullet.GetComponent<BulletBase>();
        }

        private EntityType parentEntityType
        {
            get => this.transform.parent.gameObject.GetComponent<Entity>().entityType;
        }
        
        private List<BulletBase> bulletComponents
        {
            get => bullets.Select(x => x.GetComponent<BulletBase>()).ToList();
        }
        
        private BulletPool bulletPool;
        private void Awake()
        {
            bulletPool = GameObject.FindGameObjectWithTag("BulletPool").GetComponent<BulletPool>();
            GrabObjectsFromPool();
        }

        public void ShootWeaponFromSpawner(EntityType entityType)
        {
            loadedBulletComponent.SetProperties(
                bulletType,
                angle,
                speed,
                magnitude,
                linearVelocity,
                radii,
                entityType,
                this.transform.position
                );
            loadedBullet.SetActive(true);
        }

        public void SetDamage(float damage)
        {
            foreach (var bullet in bulletComponents)
            {
                bullet.SetBulletDamage(damage);
            }
        }
        
        private void GrabObjectsFromPool()
        {
            
            for (int i = 0; i < bulletCount; i++)
            {
                var bullet = bulletPool.GetLastBulletAndRemove(parentEntityType);
                bullets.Add(bullet);
            }
        }

        private void ReturnBulletsToPool()
        {
            if (bulletPool.IsDestroyed() || bulletPool.IsUnityNull()) return;
            var inactiveBullets = bullets.Where(x => !x.activeSelf);
            foreach (var bullet in inactiveBullets)
            {
                if (bullet == null || bullet.IsDestroyed()) continue;
                bullet.SetActive(false);
            }
            bulletPool.AddBulletsToPool(bullets, parentEntityType);
        }
        private void OnDestroy()
        {
            if(Application.isPlaying)
                ReturnBulletsToPool();
        }
    }
    
    
#if UNITY_EDITOR
    [CustomEditor(typeof(BulletSpawner))]
    public class Bullet_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (BulletSpawner)target;

            if (!script.isTypeZigZagOrCurved)
                return;
            
            if(script.bulletType == BulletBase.BulletType.ZigZag)
            {
                script.magnitude = EditorGUILayout.FloatField("Magnitude", script.magnitude);
                script.frequency = EditorGUILayout.FloatField("Frequency", script.frequency);
                
            }
            else
            {
                script.linearVelocity = EditorGUILayout.Vector2Field("Linear Velocity", script.linearVelocity);
                script.radii = EditorGUILayout.Vector2Field("Radii", script.radii);
            }
        }
    }
#endif
}