using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Customer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer _renderer;

    private AssetReferenceSprite _spriteRef;
    private CustomerDataSO _customer;

    // Customer Status
    public enum CustomerStatus { Unserved, Serving, Served, Paid }
    private CustomerStatus _customerStatus = CustomerStatus.Unserved;

    // Cache Status
    private enum LoadingStatus { Unloaded, Loading, Ready }
    private LoadingStatus _loadingStatus = LoadingStatus.Unloaded;

    private AsyncOperationHandle<Sprite> _spriteLoadingHandle;

    private void OnDisable()
    {
        ReleasePotraitRef();
    }

    #region Addressable Potrait Reference
    public void Initialize(CustomerDataSO customerData)
    {
        if (customerData == null)
        {
            Debug.Log("[Customer] Customer data is missing");
            return;
        }

        if (_renderer == null)
        {
            Debug.Log("[Customer] Sprite renderer is missing");
        }

        _customer = customerData;

        LoadPotraitRef();
    }

    private void LoadPotraitRef()
    {
        if (_loadingStatus != LoadingStatus.Unloaded) return;
        if (_customer.CustomerPotraitRef == null || !_customer.CustomerPotraitRef.RuntimeKeyIsValid())
        {
            Debug.Log($"[Customer] No valid potrait in {_customer.CustomerName}");
            return;
        }

        _loadingStatus = LoadingStatus.Loading;

        _spriteRef = _customer.CustomerPotraitRef;

        _spriteLoadingHandle = _spriteRef.LoadAssetAsync();
        _spriteLoadingHandle.Completed += OnSpriteLoaded;
    }

    private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _loadingStatus = LoadingStatus.Ready;

            _renderer.sprite = handle.Result;
        }
        else
        {
            _loadingStatus = LoadingStatus.Unloaded;
            Debug.Log("[Customer] Failed to load customer potrait references");
        }
    }

    private void ReleasePotraitRef()
    {
        if (_loadingStatus == LoadingStatus.Ready)
        {
            _spriteRef.ReleaseAsset();
        }
        else if (_loadingStatus == LoadingStatus.Loading)
        {
            _spriteLoadingHandle.Completed -= OnSpriteLoaded;
            Addressables.Release(_spriteLoadingHandle);
        }

        _loadingStatus = LoadingStatus.Unloaded;
        if (_renderer != null) _renderer.sprite = null;
    }
    #endregion

    public void SetCustomerStatus(CustomerStatus status)
    {
        switch (status)
        {
            case CustomerStatus.Unserved:
                _customerStatus = CustomerStatus.Unserved; break;
            case CustomerStatus.Serving:
                _customerStatus = CustomerStatus.Serving; break;
            case CustomerStatus.Served:
                _customerStatus = CustomerStatus.Served;
                CustomerServed();
                break;
        }
    }

    public CustomerStatus GetCustomerStatus()
    {
        return _customerStatus;
    }

    private void CustomerOrder()
    {
        // To-do order related thing here
    }

    private void CustomerServed()
    {
        if (_customerStatus != CustomerStatus.Served) return;

        _customerStatus = CustomerStatus.Served;
        Debug.Log("CUSTOMER SERVED AND PAY");

        CustomerPay(50, 2);
    }

    private void CustomerPay(float totalIncome, float totalRepGain)
    {
        if (_customerStatus == CustomerStatus.Served)
        {
            EventBus<GoldChangeEvent>.Publish(new GoldChangeEvent(totalIncome));
            EventBus<RepChangeEvent>.Publish(new RepChangeEvent(totalRepGain));
            Debug.Log($"CUSTOMER PAID {totalIncome} gold and gain {totalRepGain} reputation");

            _customerStatus = CustomerStatus.Paid;
        }
    }
}