using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "HerbProject/Ingredient")]
public class IngredientSO : ScriptableObject
{
    public string IngredientName;
    public string IngredientDescription;
    public Sprite IngredientIcon;
    public float IngredientBasePrice;
}
