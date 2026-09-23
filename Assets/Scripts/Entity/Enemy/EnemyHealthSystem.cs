using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamagable
{
    public void Damage(float damage)
    {
        Destroy(gameObject);
    }
}
