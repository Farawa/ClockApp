using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AlarmSettings : MonoBehaviour
{
    [SerializeField] private AudioSource alarmSource;
    [SerializeField] ClockController clockController;
    [SerializeField] private Button alarmSetButton;
    [SerializeField] private GameObject settings;
    [SerializeField] private Button setFromClock;
    [SerializeField] private InputTime inputTime;
    [SerializeField] private ClockDisplay clockDisplay;
    private bool isNeelPlayAlarm = false;
    private DateTime targetAlarmTime;

    private void Start()
    {
        alarmSetButton.onClick.AddListener(OpenSettings);
        inputTime.SetButtonAction(SetInputAlarm);
        setFromClock.onClick.AddListener(SetClockAlarm);
        alarmSetButton.gameObject.SetActive(true);
        settings.SetActive(false);
    }

    private void OpenSettings()
    {
        alarmSetButton.gameObject.SetActive(false);
        settings.SetActive(true);
        clockController.StopMoveArrows();
    }

    private void CloseSettings()
    {
        alarmSetButton.gameObject.SetActive(true);
        settings.SetActive(false);
        clockController.ResumeTimeClock();
    }

    private async void SetClockAlarm()
    {
        clockController.SetTime(DateTime.Now);
        targetAlarmTime = clockDisplay.GetCurrentTimeFromArrows();
        CloseSettings();
        await Task.Delay(1000);
        isNeelPlayAlarm = true;
    }

    private void SetInputAlarm()
    {
        if (inputTime.TrySetTime(out var dateTime))
        {
            targetAlarmTime = dateTime;
            isNeelPlayAlarm = true;
            CloseSettings();
        }
    }

    private void Update()
    {
        if (!isNeelPlayAlarm)
            return;
        if (clockController.currentTime.Hour%12 == targetAlarmTime.Hour%12 &&
            clockController.currentTime.Minute == targetAlarmTime.Minute &&
            clockController.currentTime.Second == targetAlarmTime.Second)
        {
            PLAYALAAAARM();
            isNeelPlayAlarm = false;
        }
    }

    private void PLAYALAAAARM()
    {
        alarmSource.Play();
        isNeelPlayAlarm = false;
        Debug.Log("alarm");
    }
}
