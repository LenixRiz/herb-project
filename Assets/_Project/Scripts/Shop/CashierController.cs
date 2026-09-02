using UnityEngine;

public class CashierController : MonoBehaviour
{
    // Events
    public event System.Action<bool> OnCustomerServed;

    [Header("Dependencies")]
    private ShopManager _shopManager;
    [SerializeField] private CustomerSpawner _customerSpawner;

    private void Awake()
    {
        _shopManager = ShopManager.Instance;
    }

    private void OnEnable()
    {
        _customerSpawner.OnCustomerArrived += OnCustomerArrived;
    }

    private void OnDisable()
    {
        _customerSpawner.OnCustomerArrived -= OnCustomerArrived;
    }

    [ContextMenu("Begin Service")]
    private void OnCustomerArrived(CustomerController customerData)
    {
        bool isServed = false;
        OnCustomerServed?.Invoke(isServed);
    }

    [ContextMenu("Complete Service")]
    private void CompleteService()
    {
        bool isServed = true;
        OnCustomerServed?.Invoke(isServed);
        Debug.Log("Complete Service Triggered");
    }
}