using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform hourArrow;
    [SerializeField] private RectTransform minuteArrow;
    [SerializeField] private RectTransform secondArrow;
    [SerializeField] private TextMeshProUGUI timeText;

    public void SetTime(float hours, float minutes, float seconds)
    {
        hourArrow.rotation = GetRotationFromValue(hours%12, 12);
        minuteArrow.rotation = GetRotationFromValue(minutes, 60);
        secondArrow.rotation = GetRotationFromValue(seconds, 60);
        timeText.text = $"{hours}:{minutes}:{seconds}";
    }

    private Quaternion GetRotationFromValue(float value, float maxValue)
    {
        var degrees = value / maxValue * (-360);
        return Quaternion.Euler(0, 0, degrees);
    }
}
