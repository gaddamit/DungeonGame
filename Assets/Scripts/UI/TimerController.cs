using UnityEngine;
using TMPro;
using System;

public class TimerController : MonoBehaviour
{
    public bool IsPaused
    { 
        get; set;
    }
    private float _totalTime = 0;
    private TMP_Text _textTimer;
    private TimeSpan _timeSpan;

    // Start is called before the first frame update
    void Start()
    {
        IsPaused = false;

        _textTimer = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsPaused)
        {
            SetTimerText();
        }
    }

    private void SetTimerText()
    {
        _totalTime += Time.deltaTime;
        _timeSpan = TimeSpan.FromSeconds(_totalTime);

        // Support for days should be enough
        if(_timeSpan.Days > 0)
        {
            _textTimer.text = string.Format("Timer:{0}d {1:D2}:{2:D2}:{3:D2}", _timeSpan.Days, _timeSpan.Hours, _timeSpan.Minutes, _timeSpan.Seconds);
        }
        else
        {
            _textTimer.text = string.Format("Timer:{0:D2}:{1:D2}:{2:D2}", _timeSpan.Hours, _timeSpan.Minutes, _timeSpan.Seconds);
        }
    }

    public void ResetTimer(bool shouldStartTimer = true)
    {
        _totalTime = 0;
        SetTimerText();

        IsPaused = !shouldStartTimer;
    }
}
