public interface IDamageable
{
    void TakeDamage(float damage,float position,string playerName);
}
public interface IDeath
{
    void Death();
}