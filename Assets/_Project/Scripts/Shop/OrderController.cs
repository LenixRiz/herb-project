using UnityEditorInternal;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    [SerializeField] private CustomerSpawner _customerSpawner;
    [SerializeField] private DirectOrderUI _directOrderUI;

    private CustomerController _currentCustomer;

    private string _recipeName;

    public string RecipeName => _recipeName;

    private void OnEnable()
    {
        _customerSpawner.OnCustomerArrived += HandleCustomerArrived;
    }

    private void OnDisable()
    {
        _customerSpawner.OnCustomerArrived -= HandleCustomerArrived;
    }

    private void HandleCustomerArrived(CustomerController currentCustomer)
    {
        _currentCustomer = currentCustomer;

        CustomerOrderType orderType = currentCustomer.OrderType;
        
        switch (orderType)
        {
            case CustomerOrderType.DirectOrder:
                DirectOrder();
                break;
            case CustomerOrderType.SymptomBasedOrder:
                SymptomBasedOrder();
                break;
        }
    }

    #region order
    private void DirectOrder()
    {
        DirectOrderUI directOrderUI = _directOrderUI;
        if (_directOrderUI == null)
        {
            return;
        }

        RecipeSO currentRecipe = _currentCustomer.CustomerRecipe;
        if (currentRecipe == null)
        {
            return;
        }

        _recipeName = currentRecipe.MedicineName;

        _directOrderUI.UpdateRecipeNameUI(_recipeName);
        _directOrderUI.SetDirectOrderVisibility(true);
        GetIngredientsFromRecipe(currentRecipe);
        
    }
    #endregion

    private void SymptomBasedOrder()
    {
        _directOrderUI.SetDirectOrderVisibility(false);
    }

    private void GetIngredientsFromRecipe(RecipeSO currentRecipe)
    {
        IngredientSO[] ingredients = currentRecipe.RequiredIngredients;

        _directOrderUI.UpdateIngredientUI(ingredients);
    }
}