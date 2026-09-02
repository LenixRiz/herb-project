using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "NewCustomerData", menuName = "HerbProject/CustomerData")]
// Data statis
public class CustomerDataSO : ScriptableObject
{
    public string CustomerId;
    public string CustomerName;
    public AssetReferenceSprite CustomerPotraitRef;
    public RecipeSO TargetRecipe;
    public float MaxWaitDuration;

    // Encapsulation of enum    
    public enum CustomerType
    {
        Poor,
        Middle,
        Rich,
    }

    public enum CustomerOrderType
    {
        DirectOrder,
        SymptomBasedOrder,
    }

    [SerializeField] private CustomerType _customerType;
    [SerializeField] private CustomerOrderType _customerOrderType;
    public CustomerType CustType => _customerType;
    public CustomerOrderType CustOrderType => _customerOrderType;
}

//// Data dinamis saat runtime, saat runtime ini akan dirun
//public class ActiveCustomerSession
//{
//    public CustomerDataSO Blueprint { get; } // Get Data
//    public float RemainingWaitTime { get; set; } // Wait Duration
//    public bool IsServed { get; set; }

//    // Constructor
//    public ActiveCustomerSession(CustomerDataSO blueprint)
//    {
//        Blueprint = blueprint;
//        RemainingWaitTime = blueprint.MaxWaitDuration;
//        IsServed = false;
//    }
//}