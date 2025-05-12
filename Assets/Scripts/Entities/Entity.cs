using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bullet;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Weapon;

namespace Entities
{
    [Serializable]
    public class SpawnerProperty
    {
        public Vector2 spawnerLocation;
        public int spawnerAngle;
        public GameObject spawnerPrefab;
    }
    public enum EntityType
    {
        Player,
        Enemy
    }
    public abstract class Entity : MonoBehaviour
    {
        public float health;
        public float damage;
        public float bulletSpawnInterval;
        public EntityType entityType;
        
        public List<SpawnerProperty> spawnerProps = new List<SpawnerProperty>();
        
        protected List<ISpawner> weaponSpawners =  new List<ISpawner>();
        protected float bulletSpawnTimer = 0f;
        
        
        protected float xMin, xMax;
        protected float yMin, yMax;
        public virtual void Start()
        {
            InstantiateWeaponSpawnersInLocation();
            SetDamageToSpawners();
            SetMaxBasedOnCameraBounds();
        }
        
        protected virtual void Shoot()
        {
            
        }

        protected virtual void Update()
        {
            CheckDeath();
        }

        protected void SetDamageToSpawners()
        {
            foreach (var bulletSpawner in weaponSpawners)
            {
                bulletSpawner.SetDamage(damage);
            }
        }
        protected virtual void CheckDeath()
        {
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void SetMaxBasedOnCameraBounds()
        {
            
            var spriteSize = GetComponent<SpriteRenderer>().bounds.size.x * .5f; // Working with a simple box here, adapt to you necessity
            var cam = Camera.main;// Camera component to get their size, if this change in runtime make sure to update values
            var camHeight = cam.orthographicSize;
            var camWidth = cam.orthographicSize * cam.aspect;

            yMin = -camHeight + spriteSize; // lower bound
            yMax = camHeight - spriteSize; // upper bound
        
            xMin = -camWidth + spriteSize; // left bound
            xMax = camWidth - spriteSize; // right bound 
        }
        
        //Instantiate weapon spawners in designated location relative to parent entity
        private void InstantiateWeaponSpawnersInLocation()
        {
            foreach (var spawnerProp in spawnerProps)
            {
                var spawnerObj = Instantiate(spawnerProp.spawnerPrefab, transform);
                spawnerObj.transform.position = (Vector2)transform.position + spawnerProp.spawnerLocation;
                spawnerObj.GetComponent<BulletSpawner>().angle = spawnerProp.spawnerAngle;
                weaponSpawners.Add(spawnerObj.GetComponent<ISpawner>());
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamage damage))
            {
                switch (entityType)
                {
                    case EntityType.Player:
                        if (damage.IsFromPlayer)
                            return;
                        health -= damage.DealDamage();
                        break;
                    case EntityType.Enemy:
                        if (!damage.IsFromPlayer)
                            return;
                        health -= damage.DealDamage();
                        break;
                    default:
                        return;
                }
                other.gameObject.SetActive(false);
            }
        }
    }
    
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(SpawnerProperty))]
    public class SpawnerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            float spawnerLocationWidth = position.width * 0.3f;
            float spawnerAngleWidth = position.width * 0.2f - 15f;
            float spawnerPrefabWidth = position.width * 0.5f - 5f;
        
            Rect keyRect = new Rect(position.x, position.y, spawnerLocationWidth, position.height);
            Rect valueRect = new Rect(position.x + spawnerLocationWidth + 5f, position.y, spawnerAngleWidth, position.height);
            Rect spawnerPrefabRect = new Rect(position.x + spawnerLocationWidth + spawnerAngleWidth + 10f, position.y, spawnerPrefabWidth, position.height);

            // Get properties
            SerializedProperty keyProp = property.FindPropertyRelative("spawnerLocation");
            SerializedProperty valueProp = property.FindPropertyRelative("spawnerAngle");
            SerializedProperty spawnerPrefabProp = property.FindPropertyRelative("spawnerPrefab");

            // Draw fields
            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
            EditorGUI.PropertyField(spawnerPrefabRect, spawnerPrefabProp, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
    
    [CustomEditor(typeof(Entity))]
    public class Entity_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (Entity)target;
    
    
            for (int i = 0; i < script.spawnerProps.Count; i++)
            {
                script.spawnerProps.Add(new SpawnerProperty());
                EditorUtility.SetDirty(target);
            }
        }
    }
#endif
}