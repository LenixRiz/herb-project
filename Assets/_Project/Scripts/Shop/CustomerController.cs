using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class CustomerController : MonoBehaviour
{
    public event System.Action<float> OnTimeUpdate;
    public event System.Action OnTimeEnd;
    public event System.Action <CustomerOrderType> OnOrder;

    private SpriteRenderer _spriteRenderer;
    private AsyncOperationHandle<Sprite> _spriteLoadHandle;
    private CashierController _cashierController;
    private ShopUIController _shopUIController;

    private CustomerDataSO _customerData;

    public string CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public float WaitDuration { get; private set; }
    public float RemainingWaitDuration { get; private set; }

    private bool _isServed;

    private Coroutine _timerCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void StartTimerCoroutine()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(Timer());
        }

        _timerCoroutine = StartCoroutine(Timer());
    }

    public void ApplyConfig(CustomerDataSO customerData)
    {
        if (customerData == null)
        {
            Debug.LogWarning("Customer Data is empty!"); 
            return;
        }

        _customerData = customerData;
        
        // Ambil sprite dari address secara asynchronus
        _spriteLoadHandle = _customerData.CustomerPotraitRef.LoadAssetAsync<Sprite>();

        // Tunggu hingga loading selesai, dan umumkan
        _spriteLoadHandle.Completed += OnSpriteLoaded;

        // Disamble Customer Data
        CustomerId = _customerData.CustomerId;
        CustomerName = _customerData.CustomerName;

        float randomDuration = _customerData.MaxWaitDuration - UnityEngine.Random.Range(10, 20);
        WaitDuration = Mathf.Clamp(randomDuration, 0f, customerData.MaxWaitDuration);

        GetOrderType();

        StartTimerCoroutine();
    }
    
    private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
    {
        if (_spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer is null! Cannot apply sprite.");
            return;
        }    

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // Jika sukses ubah sprite sesuai dengan dataSO
            _spriteRenderer.sprite = handle.Result;
        }
        else
        {
            Debug.LogWarning("Failed to load Addressable sprite!");
        }
    }

    public void SetupCashierConnection(CashierController cashierController)
    {
        _cashierController = cashierController;

        _cashierController.OnCustomerServed += OnCustomerServed;
    }

    public void SetupShopUIConnection(ShopUIController shopUIController)
    {
        _shopUIController = shopUIController;
    }

    private void GetOrderType()
    {
        if (_customerData == null) return;

        CustomerOrderType orderType = GetRandomValue<CustomerOrderType>();

        bool isEmpty = Enum.GetNames(typeof(CustomerOrderType)).Length == 0;

        if (isEmpty) Debug.Log("Order Type is empty");

        Debug.Log($"Customer {CustomerName} has a wait duration of {WaitDuration} seconds and order type of {orderType}.");

        OnOrder?.Invoke(orderType);
    }

    private T GetRandomValue<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        int index = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(index);
    }

    private void OnCustomerServed(bool isServed)
    {
        if (_cashierController != null)
        {
            _cashierController.OnCustomerServed -= OnCustomerServed;
        }

        Debug.Log($"On Customer Served? {isServed}");

        if (!isServed) return;

        _isServed = true;

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        DespawnCustomer();
    }

    private void OnTimeEnded()
    {
        Debug.Log("Customer's wait time has ended.");
        OnTimeEnd?.Invoke();
        DespawnCustomer();
    }

    private void DespawnCustomer()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_cashierController != null)
        {
            _cashierController.OnCustomerServed -= OnCustomerServed;
        }

        // Lepaskan resource addressable saat objek dihancurkan
        if (_spriteLoadHandle.IsValid())
        {
            Addressables.Release(_spriteLoadHandle);
        }
    }

    private IEnumerator Timer()
    {
        float waitDuration = WaitDuration;

        while (waitDuration > 0)
        {
            waitDuration -= Time.deltaTime;

            RemainingWaitDuration = waitDuration;

            OnTimeUpdate?.Invoke(RemainingWaitDuration);
            // Announce the remaining time to the cashier controller
            yield return null;
        }
        OnTimeEnded();
    }
}