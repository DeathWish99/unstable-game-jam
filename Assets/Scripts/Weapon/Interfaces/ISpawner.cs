using Entities;

namespace Weapon
{
    public interface ISpawner
    {
        void ShootWeaponFromSpawner(EntityType entityType);
        void SetDamage(float damage);
    }
}