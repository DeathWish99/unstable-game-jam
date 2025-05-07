using Bullet;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IDamage
{
    [SerializeField]
    protected float damage;
    public float Damage { get => damage; set => damage = value; }

    protected float dotRate;
    public enum DamageType
    {
        Full,
        DOT
    }
    protected DamageType damageType;

    public enum ShotFrom
    {
        Player,
        Enemy
    }
    protected ShotFrom shotFrom;
    
    public virtual float DealDamage(float damage)
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
}
