using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimebarUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Image _timeBar;
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private CanvasGroup _timeBarPanel;

    private float _maxDuration;

    public void SetTimebarVisibility(bool isServed)
    {
        ToggleTimebarUI(!isServed); // false = show, true = hide
    }

    public void SetMaxDuration(float maxDuration)
    {
        _maxDuration = maxDuration;
    }

    public void SetDurationRemaining(float durationRemaining)
    {
        _testText.text = $"Time Remaining: {durationRemaining:F2} seconds";

        UpdateTimebarUI(durationRemaining);
    }

    private void UpdateTimebarUI(float durationRemaining)   
    {
        _timeBar.fillAmount = durationRemaining / _maxDuration; // ex: 30/60 = 0.5 or 50% fill
    }

    private void ToggleTimebarUI(bool isVisible)
    {
        _timeBarPanel.alpha = isVisible ? 1 : 0;
        _timeBarPanel.interactable = isVisible;
        _timeBarPanel.blocksRaycasts = isVisible;
    }
}