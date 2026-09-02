using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public readonly struct ServiceEvaluation
    {
        public readonly string Name;
        public readonly bool IsCorrectMedication;
        public readonly float MoneyEarned;
        public readonly float ReputationEarned;

        public ServiceEvaluation(string name, bool isCorrect, float moneyChange, float repChange)
        {
            Name = name;
            IsCorrectMedication = isCorrect;
            MoneyEarned = moneyChange;
            ReputationEarned = repChange;
        }

        public void Deconstruct(out string name, out bool isCorrect, out float moneyChange, out float repChange)
        {
            name = Name;
            isCorrect = IsCorrectMedication;
            moneyChange = MoneyEarned;
            repChange = ReputationEarned;
        }

        public string GetFeedbackSummary()
        {
            return IsCorrectMedication ? "Customer Satisfied!" : "Customer Dissapointed!";
        }
    }

    public static ShopManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float _money = 100f;
    [SerializeField] private float _reputation = 5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
