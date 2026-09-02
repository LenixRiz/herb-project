using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CustomerController : MonoBehaviour
{
    public event System.Action<float> OnTimeUpdate;
    public event System.Action OnTimeEnd;

    private SpriteRenderer _spriteRenderer;
    private AsyncOperationHandle<Sprite> _spriteLoadHandle;
    private CashierController _cashierController;
    private ShopUIController _shopUIController;

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

        // Ambil sprite dari address secara asynchronus
        _spriteLoadHandle = customerData.CustomerPotraitRef.LoadAssetAsync<Sprite>();

        // Tunggu hingga loading selesai, dan umumkan
        _spriteLoadHandle.Completed += OnSpriteLoaded;

        // Disamble Customer Data
        CustomerId = customerData.CustomerId;
        CustomerName = customerData.CustomerName;

        float randomDuration = customerData.MaxWaitDuration - Random.Range(10, 20);
        WaitDuration = Mathf.Clamp(randomDuration, 0f, customerData.MaxWaitDuration);
        Debug.Log($"Customer {CustomerName} has a wait duration of {WaitDuration} seconds.");

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

    private void OnCustomerServed(bool isServed)
    {
        if (_isServed != true) return;

        _isServed = isServed;
        _cashierController.OnCustomerServed -= OnCustomerServed;

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
        // Lepaskan resource addressable saat objek dihancurkan
        if (_spriteLoadHandle.IsValid())
        {
            Addressables.Release(_spriteLoadHandle);
        }

        Destroy(gameObject);
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