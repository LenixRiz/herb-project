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

[CreateAssetMenu(fileName = "NewCustomerData", menuName = "HerbProject/CustomerData")]
// Data statis
public class CustomerDataSO : ScriptableObject
{
    public string CustomerId;
    public string CustomerName;
    public AssetReferenceSprite CustomerPotraitRef;
    public RecipeSO TargetRecipe;
    public float MaxWaitDuration;

    [SerializeField] private CustomerType _customerType;
    [SerializeField] private CustomerOrderType _customerOrderType;
    public RecipeSO TargetRecipe2s => TargetRecipe;
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