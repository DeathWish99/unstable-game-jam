using Entities.Enemy;
using UnityEngine;

public interface IEnemy
{
    EnemyBase Stats {get; set;}
    void PerformMovement();
    void DestroyWhenOutOfScreen();
}
