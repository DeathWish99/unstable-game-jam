using System;
using UnityEditor;
using UnityEngine;

namespace Weapon
{
    public class BulletBase : WeaponBase
    {
        public float bulletLife = 1f;
        private float bulletLifeTimer = 0f;
        public enum BulletType
        {
            Straight,
            Curved,
            ZigZag
        }
        private BulletType type;
        
        private float speed = 1f;
        
        //Zig Zag
        private float magnitude = 0f; //Size of sine movement

        //Curved
        private Vector2 linearVelocity = new Vector2(0.5f, 0f); // Controls lateral circular motion
    
        private Vector2 radii = new Vector2(2f, 3f); // The size of the circular movement of the bullet
        
        private Vector2 spawnPoint;
        private Vector2 spawnDirection;
        private float timer;
        
        public bool isTypeZigZagOrCurved => type == BulletType.ZigZag || type == BulletType.Curved;

        private void Awake()
        {
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            transform.position = Movement();
            CheckTimerAndSetActiveFalse();
        }

        private Vector2 Movement()
        {
            float x = timer * speed * spawnDirection.x;
            float y = timer * speed * spawnDirection.y;
            switch (type)
            {
                case BulletType.Straight:
                    return new Vector2(x+spawnPoint.x, y+spawnPoint.y);
                case BulletType.Curved:
                    var curveAngle = speed * timer;
                    var pos = linearVelocity * timer;
                    return pos + new Vector2(Mathf.Cos(curveAngle) * radii.x * spawnDirection.x + spawnPoint.x, Mathf.Sin(curveAngle) * radii.y * spawnDirection.y + spawnPoint.y);
                case BulletType.ZigZag:
                    return new Vector2(Mathf.Sin(x + spawnPoint.x) * magnitude, y+spawnPoint.y);
                default:
                    return Vector2.zero;
            }
        }

        public void SetProperties(BulletType bulletType, float angle, float speed, float magnitude, Vector2 linearVelocity, Vector2 radii)
        {
            this.type = bulletType;
            this.speed = speed;
            this.magnitude = magnitude;
            this.linearVelocity = linearVelocity;
            this.radii = radii;
            
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void SetBulletDamage(float damage)
        {
            this.damage = damage;
        }
        private void CheckTimerAndSetActiveFalse()
        {
            bulletLifeTimer -= Time.deltaTime;
            if (bulletLifeTimer <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            bulletLifeTimer = bulletLife;
            this.transform.position = transform.parent.position;
            spawnPoint = transform.position;
            spawnDirection = transform.up;
            timer = 0f;
        }
    }
}