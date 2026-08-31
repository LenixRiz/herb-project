using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopManager;

public class ShopUIController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CashierCounter _cashier;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _reputationText;

    [Header("Buttons")]
    [SerializeField] private Button _diagnoseBtn;
    [SerializeField] private Button _craftBtn;

    private void OnEnable()
    {
        // Subscribe
        _cashier.OnCustomerServed += HandleServedCustomer;
    }

    private void OnDisable()
    {
        // Subscribe
        _cashier.OnCustomerServed -= HandleServedCustomer;
    }

    private void HandleServedCustomer(ServiceEvaluation evaluation)
    {
        var (name, isCorrect, moneyChange, repChange) = evaluation;

        _moneyText.text = moneyChange.ToString();
        _reputationText.text = repChange.ToString();

        Debug.Log($"name: {name}, medication correct? {isCorrect}, moneyChange by +{moneyChange}, reputation change by {repChange}");
    }

    [ContextMenu("Test UI Change")] // Quickly test a method without needing UI Component to trigger
    private void TestOnUIChange()
    {
        ServiceEvaluation dummyData = new ServiceEvaluation("Aceng", true, +20, +5);
        HandleServedCustomer(dummyData);
    }

    private void OnDiagnoseClick()
    {

    }

    private void OnCraftClick()
    {

    }
}
