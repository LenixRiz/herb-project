using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DB_RecipeDatabaseSO", menuName = "HerbProject/Database/RecipeDatabase")]
public class RecipeDatabaseSO : ScriptableObject
{
    [SerializeField] private List<RecipeSO> _allRecipes = new List<RecipeSO>();

    private Dictionary<string, RecipeSO> _recipeLookup;

    public void Initialize()
    {
        _recipeLookup = new Dictionary<string, RecipeSO>(_allRecipes.Count);

        foreach (var recipe in _allRecipes)
        {
            if (recipe != null && _recipeLookup.ContainsKey(recipe.recipeId))
            {
                _recipeLookup.Add(recipe.recipeId, recipe);
            }
        }
    }

    // initialize to get recipe from id
    public RecipeSO GetRecipeById(string id)
    {
        if (_allRecipes != null && _recipeLookup == null) Initialize();
        return _recipeLookup.TryGetValue(id, out RecipeSO recipe) ? recipe : null; ;
    }

    // Can access the recipe from list becase it randomized, no initalize needed
    public RecipeSO GetRandomRecipe()
    {
        if (_allRecipes.Count == 0) return null;
        return _allRecipes[Random.Range(0, _allRecipes.Count)];
    }
}
