using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class CustomerController : MonoBehaviour
{
    public event System.Action<float> OnTimeUpdate;
    public event System.Action OnTimeEnd;

    private SpriteRenderer _spriteRenderer;
    private AsyncOperationHandle<Sprite> _spriteLoadHandle;
    private CashierController _cashierController;
    private ShopUIController _shopUIController;
    private AudioManager _audioManager;
    private CustomerDataSO _customerData;

    public string CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public float WaitDuration { get; private set; }
    public float RemainingWaitDuration { get; private set; }
    public RecipeSO CustomerRecipe { get; private set; }
    public CustomerOrderType OrderType { get; private set; }

    private Coroutine _timerCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioManager = AudioManager.Instance;

        if (_audioManager == null)
        {
            Debug.LogWarning("AudioManager is not existing!");
        }
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

    public void SetCustomerTargetRecipe(RecipeSO recipe)
    { 
        CustomerRecipe = recipe;
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

        OrderType = orderType;
    }

    // Randomizing enum, by creating a method with return T (whatever ur enum name)
    // Ensure T is enum by "where T : Enum". Then get all value inside of the enum
    // Then randomize using Random.Range min of 0 and max according to the last values index
    // Cast the type to the enum class end send the randomized index.
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

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        DespawnCustomer();
    }

    private void OnTimeEnded()
    {
        if (_audioManager != null)
        {
            _audioManager.OnCustomerAngry();
        }
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