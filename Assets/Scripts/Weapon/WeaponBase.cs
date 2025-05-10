using System;
using Bullet;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IDamage
{
    protected float damage;
    public float Damage { get => damage; set => damage = value; }
    
    protected bool isFromPlayer;
    public bool IsFromPlayer { get => isFromPlayer; set => isFromPlayer = value; }

    [SerializeField]
    protected float dotRate;
    public enum DamageType
    {
        Full,
        DOT
    }
    [SerializeField]
    protected DamageType damageType;
    
    public virtual float DealDamage()
    {
        switch (damageType)
        {
            case DamageType.Full:
                return damage;
            case DamageType.DOT:
                return damage / dotRate;
            default:
                return damage;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        throw new NotImplementedException();
    }
}
