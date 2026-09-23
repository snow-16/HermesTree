using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamagable
{
    private EnemyData enemyData = new();

    void Awake()
    {
        DataManager.AddData(enemyData, gameObject);
        enemyData.SetHp(10);
    }

    public void Damage(float damage)
    {
        enemyData.Damage(damage);

        if(enemyData.IsDead)
        {
            Destroy(gameObject);
        }
    }
}
