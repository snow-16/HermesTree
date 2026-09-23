public class EnemyData : InstanceData
{
    private float _maxHp;
    public float MaxHp => _maxHp;

    private float _hp;
    public float Hp => _hp;
    public bool IsDead => _hp <= 0;

    public void SetHp(float maxHp)
    {
        _maxHp = _hp = maxHp;
    }

    public void Damage(float damage)
    {
        _hp -= damage;
    }
}
