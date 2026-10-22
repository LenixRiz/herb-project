using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomerDRegistryatabase", menuName = "HerbProject/Registry/CustomerRegistry")]
public class CustomerRegistry : ScriptableObject
{
    [SerializeField] private List<CustomerDataSO> _allCustomer = new List<CustomerDataSO>();
    private Dictionary<string, CustomerDataSO> _customerDict = new Dictionary<string, CustomerDataSO>();

    private List<string> _cachedCustomerKeys = new List<string>();

    private void OnEnable()
    {
        InitializeRegistry();
    }

    public void InitializeRegistry()
    {
        _customerDict.Clear();
        _cachedCustomerKeys.Clear();

        if (_allCustomer.Count <= 0)
        {
            Debug.LogWarning($"[CustomerRegistry] No customer listed in registry.");
            return;
        }
            
        foreach (var customer in _allCustomer)
        {
            if (customer == null) continue;

            if (!_customerDict.ContainsKey(customer.Id))
            {
                _customerDict.Add(customer.Id, customer);
                Debug.Log($"[CustomerRegistry] Add new customer called {customer.CustomerName}");
            }
            else
            {
                Debug.Log($"[CustomerRegistry] Identical customer id is found, {customer.Id}");
            }
        }

        _cachedCustomerKeys.AddRange(_customerDict.Keys);
    }

    public CustomerDataSO GetCustomerById(string id)
    {
        if (_customerDict.TryGetValue(id, out CustomerDataSO customer))
        {
            return customer;
        }
        else
        {
            Debug.Log($"[CustomerRegistry] Customer id {id} not found.");
            return null;
        }
    }

    public CustomerDataSO GetRandomCustomer()
    {
        // Take all keys from dict and make it into new List
        if (_cachedCustomerKeys.Count <= 0)
        {
            Debug.Log($"[CustomerRegistry] Cached customer keys is {_cachedCustomerKeys.Count}");
            return null;
        }

        string randomKey = _cachedCustomerKeys[UnityEngine.Random.Range(0, _cachedCustomerKeys.Count)];
        var randomCustomer = _customerDict[randomKey];

        return randomCustomer;
    }
}