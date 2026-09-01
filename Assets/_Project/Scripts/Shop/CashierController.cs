using UnityEngine;
using static ShopManager;

public class CashierController : MonoBehaviour
{
    // Events
    public event System.Action<bool> OnCustomerServed;

    [ContextMenu("Complete Service")]
    private void CompleteService()
    {
        bool isServed = true;
        OnCustomerServed?.Invoke(isServed);
        Debug.Log("Complete Service Triggered");
    }
}