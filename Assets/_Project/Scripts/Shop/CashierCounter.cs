using UnityEngine;
using static ShopManager;

public class CashierCounter : MonoBehaviour
{
    // Events
    public event System.Action<ServiceEvaluation> OnCustomerServed;

    private void FinishCustomerTransaction()
    {
        string name = "Lelen";
        bool isCorrect = true;
        float price = 50f;
        float reputation = 5;

        ServiceEvaluation result = new ServiceEvaluation(name, isCorrect, price, reputation);

        OnCustomerServed?.Invoke(result);
    }
}