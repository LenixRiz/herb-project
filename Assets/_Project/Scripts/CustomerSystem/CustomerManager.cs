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

    private float _spawnInterval;
    private float _customerWaitDuration;

    private Coroutine _spawnerCoroutine;
    private GameObject _activeCustObject;
    private Customer _activeCustomer;

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
        _custRegistry.InitializeRegistry();

        GetSpawnInterval();
        _spawnerCoroutine = StartCoroutine(CustomerSpawnerCoroutine());
    }
    #endregion

    [ContextMenu("Serve Customer")]
    public void ServeCustomer()
    {
        if (_activeCustomer == null)
        {
            Debug.Log("[CustomerManager] No customer yet");
            return;
        }

        Debug.Log("Serving...");
        _activeCustomer.SetCustomerStatus(Customer.CustomerStatus.Served);
        Debug.Log($"Customer Status: {_activeCustomer.GetCustomerStatus()}");

        if (_activeCustomer.GetCustomerStatus() == Customer.CustomerStatus.Paid)
        {
            StartCoroutine(HandleCustomerDeparture(true));
        }
    }

    #region Customer Spawn
    private IEnumerator CustomerSpawnerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);

            var customer = GetRandomCustomerToSpawn();
            if (customer == null) continue;

            _customerWaitDuration = GetRandomTime(customer.MinWaitDuration, customer.MaxWaitDuration);

            SpawnCustomer(customer);
            Debug.Log($"Customer Arrived: {customer.CustomerName} duration {_customerWaitDuration}");

            yield return new WaitForSeconds(_customerWaitDuration);

            DespawnCustomer();
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
        _activeCustObject = Instantiate(_custPrefab, transform.position, Quaternion.identity);

        if (_activeCustObject.TryGetComponent<Customer>(out var customer))
        {
            _activeCustomer = customer;
            _activeCustomer.Initialize(customerData);
            _activeCustomer.SetCustomerStatus(Customer.CustomerStatus.Serving);
            Debug.Log($"Customer Status: {_activeCustomer.GetCustomerStatus()}");
        }
    
        EventBus<CustomerArrivedEvent>.Publish(new CustomerArrivedEvent());
    }

    private void DespawnCustomer()
    {
        if (_activeCustomer != null)
        {
            _activeCustomer.SetCustomerStatus(Customer.CustomerStatus.Unserved);
            Debug.Log($"Customer Status: {_activeCustomer.GetCustomerStatus()}");
        }

        StartCoroutine(HandleCustomerDeparture(false));
    }

    private void GetSpawnInterval()
    {
        _spawnInterval = GetRandomTime(_minSpawnInterval, _maxSpawnInterval);
        Debug.Log($"Next customer spawned in {_spawnInterval} seconds");
    }

    private IEnumerator HandleCustomerDeparture(bool served)
    {
        if (!served)
        {
            EventBus<CustomerAngryEvent>.Publish(new CustomerAngryEvent());
            Debug.Log("You ran out of time and customer become angry");
        }
        else
        {
            EventBus<CustomerServedEvent>.Publish(new CustomerServedEvent());
            Debug.Log("You served them well");
        }

        GetSpawnInterval();

        if (_activeCustObject != null)
        {
            Destroy(_activeCustObject);
        }
        _activeCustomer = null;

        if (_spawnerCoroutine != null)
        {
            StopCoroutine(_spawnerCoroutine);
        }
        _spawnerCoroutine = StartCoroutine(CustomerSpawnerCoroutine());

        yield return null;
    }

    private float GetRandomTime(float _min, float _max)
    {
        return UnityEngine.Random.Range(_min, _max);
    }
    #endregion
}