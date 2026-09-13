using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dependencies")]
    private PlayerHealth _playerHealth;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _movementInput;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _playerHealth.OnDeath += StopMovement;
    }

    private void OnDisable()
    {
        _playerHealth.OnDeath -= StopMovement;
    }

    private void OnMove(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _movementInput * _moveSpeed;
    }

    private void StopMovement()
    {
        _movementInput = Vector2.zero;
        _rb.linearVelocity = Vector2.zero;
    }
}