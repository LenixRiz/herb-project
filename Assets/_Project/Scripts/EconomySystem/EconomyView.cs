using System.Globalization;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EconomyView : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _reputationText;
    [SerializeField] private TextMeshProUGUI _goldDeltaText;
    [SerializeField] private TextMeshProUGUI _reputationDeltaText;

    [Header("Configuration")]
    [SerializeField] private float _deltaDuration;

    private enum DeltaState { gold, reputation }
    private GameTimer _deltaTimer = new GameTimer();

    #region Unity Lifecycle
    private void OnEnable()
    {
        EventBus<GoldUpdateEvent>.Subscribe(OnGoldUpdate);
        EventBus<RepUpdateEvent>.Subscribe(OnReputationUpdate);

        if (_goldDeltaText != null) _goldDeltaText.gameObject.SetActive(false);
        if (_reputationDeltaText != null) _reputationDeltaText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        EventBus<GoldUpdateEvent>.Unsubscribe(OnGoldUpdate);
        EventBus<RepUpdateEvent>.Unsubscribe(OnReputationUpdate);
    }
    #endregion

    private void OnGoldUpdate(GoldUpdateEvent data)
    {
        string formattedGold = data.CurrentGold.ToString("F2", CultureInfo.InvariantCulture);

        if (_goldText != null)
        {
            _goldText.text = formattedGold;

            ShowDelta(DeltaState.gold, formattedGold);
        }
        else
        {
            Debug.Log("[EconomyView] Missing gold text in inspector");
        }
    }

    private void OnReputationUpdate(RepUpdateEvent data)
    {
        string formattedReputation = data.RepGain.ToString("F1", CultureInfo.InvariantCulture);

        if (_reputationText != null)
        {
            _reputationText.text = formattedReputation;

            ShowDelta(DeltaState.reputation, formattedReputation);
        }
        else
        {
            Debug.Log("[EconomyView] Missing reputation text in inspector");
        }
    }

    private void ShowDelta(DeltaState state, string n)
    {
        switch (state)
        {
            case DeltaState.gold:
                if (_goldDeltaText != null)
                {
                    float duration = _deltaDuration;

                    _goldDeltaText.text = n;

                    StartTimer(duration);
                    if (!_deltaTimer.IsTimeUp())
                    {
                        _goldDeltaText.gameObject.SetActive(true);
                    }
                    _goldDeltaText.gameObject.SetActive(false);
                }
                break;
            case DeltaState.reputation:
                if (_reputationDeltaText != null)
                {
                    float duration = _deltaDuration;

                    _reputationDeltaText.text = n;

                    StartTimer(duration);
                    if (!_deltaTimer.IsTimeUp())
                    {
                        _reputationDeltaText.gameObject.SetActive(true);
                    }
                    _reputationDeltaText.gameObject.SetActive(false);
                }
                break;
        }
    }

    private void StartTimer(float duration)
    {
        _deltaTimer.SetTimer(duration);
        _deltaTimer.Tick();
    }
}