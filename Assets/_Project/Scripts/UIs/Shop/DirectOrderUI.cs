using TMPro;
using UnityEngine;

public class DirectOrderUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _recipeNameText; 
    [SerializeField] private TextMeshProUGUI[] _ingredientsText;

    [SerializeField] private CanvasGroup _panel; [Tooltip("A Canvas Group")]

    public void UpdateRecipeNameUI(string recipeName)
    {
        _recipeNameText.text = recipeName;
    }

    /// <summary>
    ///     Read all ingredients value and apply ui text based on what inside the array of Ingredient
    /// </summary>
    /// <param name="ingredients"></param>
    public void UpdateIngredientUI(IngredientSO[] ingredients)
    {
        // jika index ui masih masuk dalam panjang array
        for (int i = 0; i < _ingredientsText.Length; i++)
        {
            // Always check for the index first, then check if its null or not
            if (i < ingredients.Length && ingredients[i] != null)
            {
                _ingredientsText[i].text = ingredients[i].IngredientName;
                _ingredientsText[i].gameObject.SetActive(true); // Show used index ui
            }
            else
            {
                _ingredientsText[i].gameObject.SetActive(false); // Hide unused index ui
            }
        }
    }

    public void SetDirectOrderVisibility(bool toggle)
    {
        if (toggle)
        {
            ToggleDirectOrderUI(toggle);
            return;
        }

        ToggleDirectOrderUI(false);
    }

    private void ToggleDirectOrderUI(bool isVisible)
    {
        _panel.alpha = isVisible ? 1 : 0;
        _panel.interactable = isVisible;
        _panel.blocksRaycasts = isVisible;
    }
}
