using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CustomerDatabaseSO _customerDatabase;
    [SerializeField] private GameObject _customerPrefab;

    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = AudioManager.Instance;
    }

    private void Start()
    {
        StartCoroutine(SpawnRandomCustomer());
    }

    private IEnumerator SpawnRandomCustomer()
    {
        float wait = 5f;
        yield return new WaitForSecondsRealtime(wait);

        var currentCustomerData = _customerDatabase.GetRandomCustomer();
        Debug.Log($"current customer {currentCustomerData}");


        GameObject spawnedCustomer = Instantiate(_customerPrefab, transform.position, Quaternion.identity);

        CustomerController controller = spawnedCustomer.GetComponent<CustomerController>();

        controller.ApplyConfig(currentCustomerData);

        _audioManager.OnCustomerArrived();
    }
}