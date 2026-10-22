using UnityEngine;

[CreateAssetMenu(fileName = "EconomyConfig", menuName = "HerbProject/GameConfig/EconomyConfig")]
public class EconomyConfigSO : ScriptableObject
{
    [Header("Starting Balance")]
    [Tooltip("Starting gold on new game session")]
    [SerializeField] private float _startingGold = 50f;
    [Tooltip("Startin reputation")]
    [SerializeField] private float _startingReputation = 10f;

    [Header("Constraints")]
    [Tooltip("Max gold player can store")]
    [SerializeField] private float _minGoldCap = -500;
    [SerializeField] private float _maxGoldCap = 9999;
    [Tooltip("Max reputation player can gain")]
    [SerializeField] private float _minReputationCap = 0;
    [SerializeField] private float _maxReputationCap = 999;

    // Getters
    public float StartingGold => _startingGold;
    public float StartingReputation => _startingReputation;
    public float MinGoldCap => _minGoldCap;
    public float MaxGoldCap => _maxGoldCap;
    public float MinReputationCap => _minGoldCap;
    public float MaxReputationCap => _maxReputationCap;
}