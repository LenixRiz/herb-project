using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CustomerDatabaseSO _customerDatabase;
    [SerializeField] private GameObject _customerPrefab;
    [SerializeField] private CashierController _cashierController;

    private AudioManager _audioManager;

    private bool _isServed;

    private void OnEnable()
    {
        _cashierController.OnCustomerServed += HandleCustomerServed;
    }

    private void OnDisable()
    {
        _cashierController.OnCustomerServed -= HandleCustomerServed;
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        StartCoroutine(SpawnRandomCustomer());
    }

    private void HandleCustomerServed(bool isServed)
    {
        _isServed = isServed;
        Debug.Log($"Customer served status: {_isServed}");
    }

    private IEnumerator SpawnRandomCustomer()
    {
        do
        {
            yield return new WaitForSecondsRealtime(2f);

            // Take a random customer data from the database
            var currentCustomerData = _customerDatabase.GetRandomCustomer();

            if (currentCustomerData == null)
            {
                Debug.LogWarning("Current Customer Data is empty!");
                yield break;
            }

            // Create a new session for the current customer
            GameObject spawnedCustomer = Instantiate(_customerPrefab, transform.position, Quaternion.identity);
            CustomerController controller = spawnedCustomer.GetComponent<CustomerController>();

            if (controller != null)
            {
                // Send current data to CustomerController.cs
                controller.ApplyConfig(currentCustomerData);
                // Connect the cashier controller to the customer controller
                controller.SetupCashierConnection(_cashierController);
                Debug.Log("Current Customer Data sent to CustomerController!");
            }

            if (_audioManager != null)
            {
                _audioManager.OnCustomerArrived();
                Debug.Log("Customer has arrived!");
            }
            else
            {
                Debug.LogWarning("AudioManager is not assigned in CustomerSpawner!");
            }

            yield return new WaitUntil(() => _isServed == true);

            yield return new WaitForSecondsRealtime(2f);

            StartCoroutine(SpawnRandomCustomer());

            _isServed = false; // Reset the served status for the next customer
        }
        while (_isServed);
    }
}