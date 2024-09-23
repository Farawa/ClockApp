using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ClockController : MonoBehaviour
{
    public static bool isCanMoveArrows = false;
    [SerializeField] private string timeURL = "https://yandex.com/time/sync.json";
    [SerializeField] private ClockDisplay clockDisplay;
    [SerializeField] private int utcOffset = 3;
    [SerializeField] private Button moveArrowsButton;
    private DateTime currentTime;
    private Coroutine realtimeCoro;

    private void Start()
    {
        currentTime = DateTime.Now;
        StartCoroutine(UpdateTimeFromUrl());
        StartCoroutine(EveryHourUpdate());
        StartRealtimeClock();
        moveArrowsButton.onClick.AddListener(SetCanMoveArrows);
    }

    public void SetTime(DateTime dateTime)
    {
        currentTime = dateTime;
    }

    private void SetCanMoveArrows()
    {
        StopRealtimeClock();
        isCanMoveArrows = true;

        moveArrowsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Click to resume";
        moveArrowsButton.onClick.RemoveAllListeners();
        moveArrowsButton.onClick.AddListener(StopMoveArrows);
    }

    private void StopMoveArrows()
    {
        isCanMoveArrows = false;
        currentTime = clockDisplay.GetCurrentTimeFromArrows();
        StartRealtimeClock();

        moveArrowsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Click to move arrows";
        moveArrowsButton.onClick.RemoveAllListeners();
        moveArrowsButton.onClick.AddListener(SetCanMoveArrows);
    }

    private IEnumerator EveryHourUpdate()
    {
        while (gameObject)
        {
            yield return new WaitForSecondsRealtime(3600);
        }
    }

    private void UpdateClock()
    {
        clockDisplay.SetTime(currentTime.Hour, currentTime.Minute, currentTime.Second);
    }

    #region TimeFromUrl
    private IEnumerator UpdateTimeFromUrl()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(timeURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ParseResult(webRequest.downloadHandler.text);
            }
            else
            {
                currentTime = DateTime.Now;
            }
        }
    }

    private void ParseResult(string result)
    {
        string pattern = "[0-9]{5,}";
        var regexResult = Regex.Match(result, pattern).ToString();
        var totalSeconds = regexResult.Remove(regexResult.Length - 3, 3);
        currentTime = new DateTime().AddSeconds(double.Parse(totalSeconds)).AddHours(utcOffset);
    }
    #endregion

    #region RealtimeClock
    private void StopRealtimeClock()
    {
        StopCoroutine(realtimeCoro);
        realtimeCoro = null;
    }

    private void StartRealtimeClock()
    {
        if (realtimeCoro != null) return;
        realtimeCoro = StartCoroutine(RealtimeClockCoro());
    }

    private IEnumerator RealtimeClockCoro()
    {
        while (gameObject)
        {
            yield return new WaitForSecondsRealtime(1);
            currentTime = currentTime.AddSeconds(1d);
            UpdateClock();
        }
    }
    #endregion
}
