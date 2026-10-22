using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [SerializeField] private EconomyConfigSO _config;

    // Runtime
    private float _currentGold;
    private float _currentReputation;

    public float CurrentGold => _currentGold;
    public float CurrentReputation => _currentReputation;

    #region Unity Lifecycle
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeEconomy();
    }

    private void OnEnable()
    {
        EventBus<GoldChangeEvent>.Subscribe(OnGoldChanged);
        EventBus<RepChangeEvent>.Subscribe(OnReputationChanged);
    }

    private void OnDisable()
    {
        EventBus<GoldChangeEvent>.Unsubscribe(OnGoldChanged);
        EventBus<RepChangeEvent>.Unsubscribe(OnReputationChanged);
    }
    #endregion

    private void InitializeEconomy()
    {
        if (_config == null)
        {
            Debug.LogError("[EconomyManager] EconomyConfigSO is not attached to inspector");
        }

        _currentGold = _config.StartingGold;
        _currentReputation = _config.StartingReputation;
    }

    #region Business and Economy Logic
    private void OnGoldChanged(GoldChangeEvent data)
    {
        float previousGold = _currentGold;
        _currentGold = Mathf.Clamp(_currentGold + data.GoldIncome, _config.MinGoldCap, _config.MaxGoldCap);

        float actualDelta = _currentGold - previousGold;

        if (!Mathf.Approximately(actualDelta, 0))
        {
            EventBus<GoldUpdateEvent>.Publish(new GoldUpdateEvent(CurrentGold, actualDelta));
        }
    }

    private void OnReputationChanged(RepChangeEvent data)
    {
        float previousRep = _currentReputation;
        _currentReputation = Mathf.Clamp(_currentReputation + data.RepGain, _config.MinReputationCap, _config.MaxReputationCap);

        float actualDelta = _currentReputation - previousRep;

        if (!Mathf.Approximately(_currentReputation, 0))
        {
            EventBus<RepUpdateEvent>.Publish(new RepUpdateEvent(CurrentReputation, actualDelta));
        }
    }
    #endregion
}