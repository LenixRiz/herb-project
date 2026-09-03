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

        RecipeSO currentRecipe = _currentCustomer.CustomerRecipe;
        _recipeName = currentRecipe.name;
        _directOrderUI.UpdateRecipeNameUI(_recipeName);

        switch (orderType)
        {
            case CustomerOrderType.DirectOrder:
                GetIngredientsFromRecipe(currentRecipe);
                break;
            case CustomerOrderType.SymptomBasedOrder:
                Debug.Log("Sakit jir");
                break;
        }
    }

    private void GetIngredientsFromRecipe(RecipeSO currentRecipe)
    {
        IngredientSO[] ingredients = currentRecipe.RequiredIngredients;

        _directOrderUI.UpdateIngredientUI(ingredients);
    }
}