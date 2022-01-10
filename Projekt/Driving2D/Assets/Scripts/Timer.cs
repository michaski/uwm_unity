using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI TimerText;

    [NonSerialized]
    public bool IsRunning = false;

    [NonSerialized] 
    public Action OnTimerReset;
    [NonSerialized]
    public Action OnTimerFinished;

    private const float TIMER_INITIAL_SECONDS = 1.0f;
    private const float TIMER_INITIAL_MINUTES = 4.0f;
    private float _remainingSeconds = TIMER_INITIAL_SECONDS;
    private float _remainingMinutes = TIMER_INITIAL_MINUTES;

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void PauseTimer()
    {
        IsRunning = false;
    }

    public void ResetTimer()
    {
        OnTimerReset();
        IsRunning = false;
        _remainingSeconds = TIMER_INITIAL_SECONDS;
        _remainingMinutes = TIMER_INITIAL_MINUTES;
        TimerText.color = Color.white;
    }

    void Update()
    {
        if (IsRunning)
        {
            if (_remainingSeconds <= 0.0f)
            {
                if (_remainingMinutes == 0.0f)
                {
                    TimerFinished();
                    return;
                }
                _remainingMinutes--;
                _remainingSeconds = 60.0f;
            }
            _remainingSeconds -= Time.deltaTime;
            TimerText.text = $"Remaining Time: {_remainingMinutes:00}:{(int)_remainingSeconds:00}";
            if (_remainingMinutes == 0.0f && _remainingSeconds <= 30.0f)
            {
                TimerText.color = ((int)_remainingSeconds % 2 == 0) ? Color.red : Color.white;
            }
        }
    }

    void TimerFinished()
    {
        OnTimerFinished();
        IsRunning = false;
    }
}
