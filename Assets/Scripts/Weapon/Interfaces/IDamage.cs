namespace Bullet
{
    public interface IDamage
    {
       public float Damage { get; set; }
       public bool IsFromPlayer { get; set; }
       public float DealDamage();
       
       
    }
}