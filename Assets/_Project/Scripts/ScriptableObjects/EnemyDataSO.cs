using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "HerbProject/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public string name;
    public SpriteRenderer spriteRenderer;
    public string description;
    public float hp;
    public float damage;
    public float speed;
}
