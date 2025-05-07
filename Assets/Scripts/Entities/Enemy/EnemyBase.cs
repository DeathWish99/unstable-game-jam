using System.Collections.Generic;
using UnityEngine;

namespace Entities.Enemy
{
    [System.Serializable]
    public class EnemyBase
    {
        public List<Vector3> directions;
        public float movementSpeed;
        public float bulletDelayTimer;
        public float bulletShootingTimer;
        public float directionTimer;
    }
}