using DG.Tweening;
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
    [Range(0.1f, 1f)]
    [SerializeField] private float clockMoveDuration = 0.5f;
    [SerializeField] private Ease easeType = Ease.Linear;

    public void SetTime(float hours, float minutes, float seconds)
    {
        RotateArrow(hourArrow, GetRotationFromValue(hours % 12, 12));
        RotateArrow(minuteArrow, GetRotationFromValue(minutes, 60));
        RotateArrow(secondArrow, GetRotationFromValue(seconds, 60));
        timeText.text = $"{hours}:{minutes}:{seconds}";
    }

    private void RotateArrow(RectTransform rect, Quaternion angle)
    {
        rect.DORotateQuaternion(angle, clockMoveDuration).SetEase(Ease.Linear);
    }

    private Quaternion GetRotationFromValue(float value, float maxValue)
    {
        var degrees = value / maxValue * (-360);
        return Quaternion.Euler(0, 0, degrees);
    }
}
