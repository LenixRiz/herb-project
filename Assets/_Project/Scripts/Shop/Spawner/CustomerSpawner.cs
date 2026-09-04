using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public event System.Action<CustomerController> OnCustomerArrived;

    [Header("Dependencies")]
    [SerializeField] private CustomerDatabaseSO _customerDatabase;
    [SerializeField] private GameObject _customerPrefab;
    [SerializeField] private CashierController _cashierController;
    [SerializeField] private ShopUIController _shopUIController;
    [SerializeField] private RecipeDatabaseSO _recipeDatabase;

    private CustomerController _currentController;

    private bool _isServed;
    private bool _isEnded;

    public bool IsServed 
    {   get
        {
            return _isServed; 
        }
        set
        {
            _isServed = value;
        }
    }

    private void OnEnable()
    {
        _cashierController.OnCustomerServed += HandleCustomerServed;
    }

    private void OnDisable()
    {
        _cashierController.OnCustomerServed -= HandleCustomerServed;
        UnsubscribeCurrentCustomer();
    }

    private void Start()
    {
        StartCoroutine(SpawnRandomCustomer());
    }

    private void HandleCustomerServed(bool isServed)
    {
        _isServed = isServed;
    }

    private void HandleCustomerTimeEnded()
    {
        UnsubscribeCurrentCustomer();
        _isEnded = true;
    }

    private void UnsubscribeCurrentCustomer()
    {
        if (_currentController != null)
        {
            _currentController.OnTimeEnd -= HandleCustomerTimeEnded;
        }
    }

    private IEnumerator SpawnRandomCustomer()
    {
        while (true)
        {
            _isServed = false; // Reset the served status for the next customer
            _isEnded = false;

            // Take a random customer data from the database
            var currentCustomerData = _customerDatabase.GetRandomCustomer();
            var targetRecipe = _recipeDatabase.GetRandomRecipe();

            if (currentCustomerData == null)
            {
                Debug.LogWarning("Current Customer Data is empty!");
                yield break;
            }

            // Create a new session for the current customer
            GameObject spawnedCustomer = Instantiate(_customerPrefab, transform.position, Quaternion.identity);
            _currentController = spawnedCustomer.GetComponent<CustomerController>();

            if (_currentController != null)
            {
                _currentController.OnTimeEnd += HandleCustomerTimeEnded;

                // Send current data to CustomerController.cs
                _currentController.ApplyConfig(currentCustomerData);
                _currentController.SetCustomerTargetRecipe(targetRecipe);
                // Connect the cashier controller to the customer controller
                _currentController.SetupCashierConnection(_cashierController);
                _currentController.SetupShopUIConnection(_shopUIController);

                // Announce subscriber, so they can react and use the customer controller reference
                OnCustomerArrived?.Invoke(_currentController);
            }

            yield return new WaitUntil(() => _isServed == true || _isEnded == true);

            yield return new WaitForSecondsRealtime(2f);
        }
    }   
}