using UnityEngine;
using UnityEngine.AddressableAssets;

public enum CustomerType
{
    Poor = 0,
    Middle = 1,
    Rich = 2,
}

public enum CustomerOrderType
{
    DirectOrder = 0,
    SymptomBasedOrder = 1,
}

[CreateAssetMenu(fileName = "NewCustomerData", menuName = "HerbProject/Customer")]
public class CustomerDataSO : IdentifiableSO
{
    [Header("Customer Identity")]
    [SerializeField] private string _customerName;
    [SerializeField] private AssetReferenceSprite _customerPotraitRef;
    [TextArea(3, 20)]
    [SerializeField] private string _customerDescription;

    [Header("Customer Type")]
    [Tooltip("Set customer financial and its order type")]
    [SerializeField] private CustomerType _customerType;
    [SerializeField] private CustomerOrderType _customerOrderType;

    [Header("Customer Order")]
    [Tooltip("Customer most ordered recipe")]
    [SerializeField] private RecipeSO _signatureOrder;
    [SerializeField] private float _minWaitDuration;
    [SerializeField] private float _maxWaitDuration;

    public string CustomerName => _customerName;
    public AssetReferenceSprite CustomerPotraitRef => _customerPotraitRef;
    public string CustomerDescription => _customerDescription;
    public CustomerType CustomerType => _customerType;
    public CustomerOrderType CustomerOrderType => _customerOrderType;
    public RecipeSO SignatureOrder => _signatureOrder;
    public float MinWaitDuration => _minWaitDuration;
    public float MaxWaitDuration => _maxWaitDuration;
}