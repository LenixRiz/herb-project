using TMPro;
using UnityEngine;

public class timecontroller : MonoBehaviour
{
    [Tooltip("How fast in-game time moves compared to real life. (e.g, 60 means 1x, 30 means 0.5x)")]
    [SerializeField] private float _timeMultiplier = 1f;
    [SerializeField] private float _setStartingHours = 8;
    private float _totalSeconds = 0f;

    [SerializeField] private TextMeshProUGUI _displayClockText;

    private void Start()
    {   
        _timeMultiplier = _timeMultiplier * 60f;
        _totalSeconds = _setStartingHours * 3600;
    }

    private void Update()
    {
        Clock();
    }

    private void Clock()
    {
        _totalSeconds += Time.deltaTime * _timeMultiplier;

        _displayClockText.text = FormatTime();
    }

    private string FormatTime()
    {
        float seconds = _totalSeconds % 60;
        float minute = (_totalSeconds / 60) % 60;
        float hours = _totalSeconds / 3600;

        return string.Format("{0:00}:{1:00}", hours, minute);
    }
}