using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance { get; private set; }

    [Header("Registry")]
    [SerializeField] private CustomerRegistry _custRegistry;

    [Header("Customer Prefab")]
    [Tooltip("Put customer prefab game object here")]
    [SerializeField] private GameObject _custPrefab;

    [Header("Spawner Config")]
    [SerializeField] private float _minSpawnInterval = 5f; // Default 5 seconds
    [SerializeField] private float _maxSpawnInterval = 15f; // Default 15 seconds

    private WaitForSeconds _spawnWait;
    private GameObject _activeCustObject;

    private bool _isServed;

    #region Unity Lifecycle
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _spawnWait = new WaitForSeconds(GetRandomSpawnInterval());

        _custRegistry.InitializeRegistry();

        StartCoroutine(CustomerSpawnerCoroutine());
    }
    #endregion

    #region Customer Spawn
    private IEnumerator CustomerSpawnerCoroutine()
    {
        var customer = GetRandomCustomerToSpawn();

        while (true)
        {
            if (customer != null)
            {
                if (_isServed) yield return null;

                yield return _spawnWait;

                SpawnCustomer(customer);

                yield return _spawnWait;

                DespawnCustomer();
            }
        }
    }

    private CustomerDataSO GetRandomCustomerToSpawn()
    {
        if (_custRegistry == null)
        {
            Debug.LogWarning("[CustomerManager] Customer registry is missing!");
            return null;
        }

        var _custData = _custRegistry.GetRandomCustomer();
        return _custData;
    }

    private void SpawnCustomer(CustomerDataSO customerData)
    {
        _isServed = false;
        _activeCustObject = Instantiate(_custPrefab, transform.position, Quaternion.identity);

        if (_activeCustObject.TryGetComponent<Customer>(out var customer))
        {
            customer.Initialize(customerData);
        }

        EventBus<CustomerArrivedEvent>.Publish(new CustomerArrivedEvent());
    }

    private void DespawnCustomer()
    {
        Destroy(_activeCustObject);
    }

    private float GetRandomSpawnInterval()
    {
        return UnityEngine.Random.Range(_minSpawnInterval, _maxSpawnInterval);
    }
    #endregion
}