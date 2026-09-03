using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CashierController _cashierController;
    [SerializeField] private CustomerSpawner _customerSpawner;
    [SerializeField] private TimebarUI _timebarUI;
    private CustomerController _currentCustomer;

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
        // Unsubscribe from events to prevent memory leaks when game is closed or scene is changed
        _customerSpawner.OnCustomerArrived -= HandleCustomerArrived;
        _cashierController.OnCustomerServed -= HandleCustomerServed;
        _currentCustomer.OnTimeUpdate -= OnTimeUpdate;
    }

    private void OnTimeUpdate(float durationRemaining)
    {
        _timebarUI.SetDurationRemaining(durationRemaining);
    }

    private void HandleCustomerArrived(CustomerController customerData)
    {
        // Unsubscribe from the previous customer's time update event if there was a previous customer
        if (_currentCustomer != null)
        {
            _currentCustomer.OnTimeUpdate -= OnTimeUpdate;
            _currentCustomer.OnTimeEnd -= HandleCustomerTimeEnd;
        }

        _currentCustomer = customerData;

        // Subscribe to new customer
        if (_currentCustomer != null)
        {
            _currentCustomer.OnTimeUpdate += OnTimeUpdate;
            _currentCustomer.OnTimeEnd += HandleCustomerTimeEnd;
        }

        _timebarUI.SetMaxDuration(_currentCustomer.WaitDuration);
        _timebarUI.SetTimebarVisibility(false); // Show timebar
    }

    private void HandleCustomerTimeEnd()
    {
        _timebarUI.SetTimebarVisibility(true);
    }

    private void HandleCustomerServed(bool isServed)
    {
        _timebarUI.SetTimebarVisibility(isServed); // Should be true to hide timebar

        // Unsubscribe after customer is served
        if (isServed == true && _currentCustomer != null)
        {
            _currentCustomer.OnTimeUpdate -= OnTimeUpdate;
        }

        // Note to self: always check for memory leak and prevent it with unsub-ing whenever the thing ends here and there
    }
}
