using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Dependencies")]
    private EnemyDataSO _enemyData;

    private float _currentHealth;
    private float _currentDamage;

    private void Awake()
    {
        _currentHealth = _enemyData.MaxHealth;
        _currentDamage = _enemyData.Damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            return;

        damageable.TakeDamage(_currentDamage);
    }
}
