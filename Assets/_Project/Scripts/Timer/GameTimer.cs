using UnityEngine;

public class GameTimer
{
    private float _currentValue;

    public void SetTimer(float duration)
    {
        if (duration <= 0)
        {
            Debug.Log("[Timer] Countdown value can not be 0 or less");
            _currentValue = 1f;
            return;
        }

        _currentValue = duration;
    }

    public float Tick()
    {
        if (_currentValue > 0)
        {
            _currentValue -= Time.deltaTime;
        }

        return _currentValue;
    }

    public bool IsTimeUp() => _currentValue <= 0;
}