using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimebarUI : MonoBehaviour
{
    private System.Action<float> OnTickUpdate;

    [SerializeField] private Image _timeBar;
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private CanvasGroup _timeBarPanel;

    private float _maxDuration;

    private void OnEnable()
    {
        HandleTogglePanel(false);

        OnTickUpdate += UpdateTimebarUI;
    }

    private void OnDisable()
    {
        OnTickUpdate -= UpdateTimebarUI;
    }

    public void SetTimebarDuration(float customerMaxDuration)
    {
        _maxDuration = customerMaxDuration;
    }

    public void StartTimebar()
    {
        StartCoroutine(Timer(_maxDuration));
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

    public void HandleTogglePanel(bool isServed)
    {
        ToggleTimebarUI(false);

        switch (isServed)
        {
            case true:
                ToggleTimebarUI(false);
                break;
            case false:
                ToggleTimebarUI(true);
                break;
            default:
        }
    }

    private IEnumerator Timer(float durationRemaining)
    {
        yield return new WaitForSecondsRealtime(2f);

        float sessionDurationRemaining = durationRemaining;
        
        while (sessionDurationRemaining > 0)
        {
            sessionDurationRemaining -= Time.unscaledDeltaTime;

            _testText.text = $"Time Remaining: {sessionDurationRemaining:F2} seconds";
            OnTickUpdate?.Invoke(sessionDurationRemaining);

            yield return null;
        }

        if (sessionDurationRemaining <= 0)
        {
            _testText.text = "Time's Up!";
            OnTickUpdate?.Invoke(0f);
            StopAllCoroutines();
        }
    }

}
