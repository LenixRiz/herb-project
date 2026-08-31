using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CustomerController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private AsyncOperationHandle<Sprite> _spriteLoadHandle;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ApplyConfig(CustomerDataSO customerData)
    {
        if (customerData == null) return;

        // Ambil sprite dari address secara asynchronus
        _spriteLoadHandle = customerData.CustomerPotraitRef.LoadAssetAsync<Sprite>();

        // Tunggu hingga loading selesai, dan umumkan
        _spriteLoadHandle.Completed += OnSpriteLoaded;

        string customerId = customerData.CustomerId;
        string customerName = customerData.CustomerName;
        CustomerDataSO.CustomerType customerType = customerData.CustType;
        var customerTargetRecipe = customerData.TargetRecipe;
        float customerWaitDuration = Mathf.Clamp(Random.Range(customerData.MaxWaitDuration - 20f, customerData.MaxWaitDuration), 0, customerData.MaxWaitDuration);

        Debug.Log($"{customerId} + {customerName} + {customerType} + {customerTargetRecipe} + {customerWaitDuration}");
    }

    private void OnSpriteLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> handle)
    {
        if (_spriteRenderer == null) return;

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

    private void OnDestroy()
    {
        if (_spriteLoadHandle.IsValid())
        {
            Addressables.Release(_spriteLoadHandle);
        }
    }
}