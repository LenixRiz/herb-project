using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CustomerController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private AsyncOperationHandle<Sprite> _spriteLoadHandle;

    private CashierController _cashierController;

    public string CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public float WaitDuration { get; private set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

        Debug.Log($"Customer Duration {WaitDuration}");
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
        _cashierController.OnCustomerServed += HandleCustomerServed;
    }

    private void HandleCustomerServed(bool isServed)
    {
        if (isServed != true) return;

        DespawnCustomer();

        _cashierController.OnCustomerServed -= HandleCustomerServed;
    }

    private void DespawnCustomer()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Lepaskan resource addressable saat objek dihancurkan
        if (_spriteLoadHandle.IsValid())
        {
            Addressables.Release(_spriteLoadHandle);
        }
    }
}