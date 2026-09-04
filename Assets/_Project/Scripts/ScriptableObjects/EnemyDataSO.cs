using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "HerbProject/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public string Name;
    public SpriteRenderer SpriteRenderer;
    public string Description;
    public float HeatlhPoint;
    public float Damage;
    public float Speed;
}
