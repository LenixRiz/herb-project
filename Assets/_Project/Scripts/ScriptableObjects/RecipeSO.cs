using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "HerbProject/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string MedicineName;
    public Sprite MedicineIcon;
    public IngredientSO[] RequiredIngredients;
    public int SellPrice;
    public int BaseReputationGain;
}