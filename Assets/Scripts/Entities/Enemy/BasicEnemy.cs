using System;
using UnityEngine;

namespace Entities.Enemy
{
    public class BasicEnemy : Entity, IEnemy
    {
        [SerializeField]
        private EnemyBase stats;
        
        public EnemyBase Stats { get => stats; set => stats = value; }
        
        private Vector2 _spawnPoint;
        private float _timer;
        private float _bulletDelayTimer;
        private float _bulletShootingTimer;
        private float _directionTimer;
        private int _directionCount;
        private bool _hasDirectionRestarted = false;

        public override void Start()
        {
            base.Start();
            _spawnPoint = transform.position;
            _timer = 0f;
            _bulletDelayTimer = stats.bulletDelayTimer;
            _bulletShootingTimer = stats.bulletShootingTimer;
            _directionTimer = stats.directionTimer;
            _directionCount = 0;
            entityType = EntityType.Enemy;
        }

        protected override void Update()
        {
            base.Update();
            PerformMovement();
            Shoot();
        }

        public void PerformMovement()
        {
            _timer += Time.deltaTime;
            _directionTimer -= Time.deltaTime;
            if (_directionTimer > 0)
            {
                if (_hasDirectionRestarted)
                    _hasDirectionRestarted = false;
                transform.position += stats.directions[_directionCount] * (stats.movementSpeed * Time.deltaTime);
            }
            else
            {
                if (_directionCount == stats.directions.Count - 1)
                {
                    _hasDirectionRestarted = true;
                    _directionCount = 0;
                }
                
                _directionTimer = stats.directionTimer;
                if(!_hasDirectionRestarted)
                    _directionCount++;
            }
        }

        protected override void Shoot()
        {
            if (_bulletDelayTimer <= 0)
            {
                _bulletShootingTimer -= Time.deltaTime;
                bulletSpawnTimer -= Time.deltaTime;

                if (bulletSpawnTimer > 0) return;

                if (_bulletShootingTimer > 0)
                {
                    bulletSpawnTimer = bulletSpawnInterval;
    
                    foreach (var spawner in weaponSpawners)
                    {
                        spawner.ShootWeaponFromSpawner(entityType);
                    }
                }
                else
                {
                    _bulletDelayTimer = stats.bulletDelayTimer;
                    _bulletShootingTimer = stats.bulletShootingTimer;
                }
            }
            else
            {
                _bulletDelayTimer -= Time.deltaTime;
            }
        }
    }
}