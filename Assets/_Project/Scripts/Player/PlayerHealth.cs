using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event System.Action<float> OnHealthChanged;
    public event System.Action OnDeath;

    [Header("Settings")]
    [SerializeField] private float _maxHealth = 100f;

    private bool _isDead = false;
    private float _currentHealth;

    public bool IsDead => _isDead;
    public float CurrentHealth => _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;

        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _currentHealth = 0;

        OnDeath?.Invoke();

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //Todo: Death sounds
    }
}