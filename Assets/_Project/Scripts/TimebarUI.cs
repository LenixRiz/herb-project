using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimebarUI : MonoBehaviour
{
    private System.Action<float> OnTimeUpdate;

    [SerializeField] private Image _timeBar;
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private CanvasGroup _timeBarPanel;

    private float _maxDuration;

    private Coroutine _currentTimerCoroutine;

    private void OnEnable()
    {
        ToggleTimebarUI(false); 

        OnTimeUpdate += UpdateTimebarUI;
    }

    private void OnDisable()
    {
        OnTimeUpdate -= UpdateTimebarUI;
    }

    public void SetTimebarDuration(float customerMaxDuration)
    {
        _maxDuration = customerMaxDuration;
    }

    public void StartTimebar()
    {
        if (_currentTimerCoroutine != null)
        {
            StopCoroutine(_currentTimerCoroutine);
        }

        _timeBar.fillAmount = 1f; // Reset the time bar to full then start the countdown
        _currentTimerCoroutine = StartCoroutine(TimerCoroutine(_maxDuration));
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

    public void SetTimebarVisibility(bool isServed)
    {
        ToggleTimebarUI(!isServed); // false = show, true = hide
    }

    private IEnumerator TimerCoroutine(float durationRemaining)
    { 
        float sessionDurationRemaining = durationRemaining;
        
        while (sessionDurationRemaining > 0)
        {
            sessionDurationRemaining -= Time.deltaTime;

            _testText.text = $"Time Remaining: {sessionDurationRemaining:F2} seconds";
            OnTimeUpdate?.Invoke(sessionDurationRemaining);

            yield return null;
        }

        _testText.text = "Time's Up!";
        OnTimeUpdate?.Invoke(0f);
    }
}