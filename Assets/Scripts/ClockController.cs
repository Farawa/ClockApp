using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class ClockController : MonoBehaviour
{
    [SerializeField] private string timeURL = "https://yandex.com/time/sync.json";
    [SerializeField] private ClockDisplay clockDisplay;
    [SerializeField] private int utcOffset = 3;
    private DateTime currentTime;

    private void Start()
    {
        currentTime = DateTime.Now;
        StartCoroutine(UpdateTimeFromUrl());
    }

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

    private void OnEnable()
    {
        StartCoroutine(RealtimeClockCoro());
    }

    private void UpdateClock()
    {
        clockDisplay.SetTime(currentTime.Hour, currentTime.Minute, currentTime.Second);
    }

    private IEnumerator RealtimeClockCoro()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(1);
            currentTime = currentTime.AddSeconds(1d);
            UpdateClock();
        }
    }
}
