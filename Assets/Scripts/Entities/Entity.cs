using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public abstract class Entity : MonoBehaviour
    {
        public float health;
        public float damage;
        public float bulletSpawnInterval;
        
        public List<SpawnerProperty> spawnerProps = new List<SpawnerProperty>();
        
        protected List<BulletSpawner> bulletSpawners =  new List<BulletSpawner>();
        protected float bulletSpawnTimer = 0f;
        public virtual void Start()
        {
            InstantiateBulletSpawnersInLocation();
        }

        protected virtual void Shoot()
        {
            
        }

        private void SetDamageToSpawners()
        {
            
        }
        
        //Instantiate bulletspawners in designated location relative to parent entity
        private void InstantiateBulletSpawnersInLocation()
        {
            foreach (var spawnerProp in spawnerProps)
            {
                var spawnerObj = Instantiate(spawnerProp.spawnerPrefab, transform);
                spawnerObj.transform.position = (Vector2)transform.position + spawnerProp.spawnerLocation;
                spawnerObj.GetComponent<BulletSpawner>().angle = spawnerProp.spawnerAngle;
                bulletSpawners.Add(spawnerObj.GetComponent<BulletSpawner>());
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