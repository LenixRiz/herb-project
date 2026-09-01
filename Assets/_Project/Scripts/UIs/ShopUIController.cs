using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;
using static ShopManager;

public class ShopUIController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CashierController _cashierController;
    [SerializeField] private CustomerSpawner _customerSpawner;
    [SerializeField] private TimebarUI _timebarUI;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _reputationText;

    [Header("Buttons")]
    [SerializeField] private Button _diagnoseBtn;
    [SerializeField] private Button _craftBtn;

    private void OnEnable()
    {
        _customerSpawner.OnCustomerArrived += HandleCustomerArrived;
        _cashierController.OnCustomerServed += HandleCustomerServed;
    }

    private void OnDisable()
    {
        _customerSpawner.OnCustomerArrived -= HandleCustomerArrived;
        _cashierController.OnCustomerServed -= HandleCustomerServed;
    }

    public void HandleCustomerArrived(CustomerDataSO customerData)
    {
        _timebarUI.SetTimebarDuration(customerData.MaxWaitDuration);
        _timebarUI.StartTimebar();
    }

    public void HandleCustomerServed(bool isServed)
    {
        _timebarUI.HandleTogglePanel(isServed);
    }
}
