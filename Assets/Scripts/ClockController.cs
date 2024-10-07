using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;

public class ClockController : MonoBehaviour
{
    public static bool isCanMoveArrows = false;
    [SerializeField] private bool isUseRosvel = true;
    [SerializeField] private int utcOffset = 3;
    [SerializeField] private ClockDisplay clockDisplay;
    public DateTime currentTime { get; private set; }
    private Coroutine realtimeCoro;

    private void Start()
    {
        currentTime = DateTime.Now;
        UpdateTime();
        StartCoroutine(EveryHourUpdate());
        StartRealtimeClock();
    }

    public void SetTime(DateTime dateTime)
    {
        currentTime = dateTime;
    }

    public void StopMoveArrows()
    {
        StopRealtimeClock();
        isCanMoveArrows = true;
        UpdateClock();
    }

    public void ResumeTimeClock()
    {
        isCanMoveArrows = false;
        currentTime = clockDisplay.GetCurrentTimeFromArrows();
        StartRealtimeClock();
    }

    private IEnumerator EveryHourUpdate()
    {
        while (gameObject)
        {
            yield return new WaitForSecondsRealtime(3600);
            if (isUseRosvel)
            {
                StartCoroutine(UpdateTimeFromUrlRoswell());
            }
            else
            {
                StartCoroutine(UpdateTimeFromUrlYandex());
            }
        }
    }

    private void UpdateClock()
    {
        clockDisplay.SetTime(currentTime.Hour, currentTime.Minute, currentTime.Second);
    }

    #region TimeFromUrl
    private void UpdateTime()
    {
        if (isUseRosvel)
        {
            StartCoroutine(UpdateTimeFromUrlRoswell());
        }
        else
        {
            StartCoroutine(UpdateTimeFromUrlYandex());
        }
    }

    private IEnumerator UpdateTimeFromUrlYandex()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://yandex.com/time/sync.json"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ParseResultYandex(webRequest.downloadHandler.text);
            }
            else
            {
                currentTime = DateTime.Now;
            }
        }
        void ParseResultYandex(string text)
        {
            string pattern = "[0-9]{5,}";
            var regexResult = Regex.Match(text, pattern).ToString();
            var totalSeconds = regexResult.Remove(regexResult.Length - 3, 3);
            currentTime = new DateTime().AddSeconds(double.Parse(totalSeconds)).AddHours(utcOffset);
        }
    }

    private IEnumerator UpdateTimeFromUrlRoswell()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://roswell.systems/"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ParseResultRoswell(webRequest.downloadHandler.text);
            }
            else
            {
                currentTime = DateTime.Now;
            }
        }
        void ParseResultRoswell(string text)
        {
            var totalSeconds = text.Split(" ")[0];
            currentTime = new DateTime().AddSeconds(double.Parse(totalSeconds)).AddHours(utcOffset);
        }
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
        if (realtimeCoro != null) StopCoroutine(realtimeCoro);
        UpdateTime();
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
