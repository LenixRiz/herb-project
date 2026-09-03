using Unity.VisualScripting;
using UnityEngine;

public class CashierController : MonoBehaviour
{
    public System.Action<bool> OnCustomerServed;

    [Header("Dependencies")]
    private ShopManager _shopManager;
    [SerializeField] private CustomerSpawner _customerSpawner;

    private bool _isServingCustomer;

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

    private void OnCustomerArrived(CustomerController customer)
    {
        _isServingCustomer = true;
    }

    [ContextMenu("Complete Service")]
    private void CompleteService()
    {
        if (!_isServingCustomer)
        {
            Debug.Log("No customer to serve right now");
        }

        bool isServed = true; // Hide timebar and continue the spawner
        Debug.Log("Complete Service Triggered");
        OnCustomerServed?.Invoke(isServed);
    }
}