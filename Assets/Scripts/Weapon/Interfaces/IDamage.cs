namespace Bullet
{
    public interface IDamage
    {
       public float Damage { get; set; }
       
       public float DealDamage(float damage, bool isPlayer);
    }
}