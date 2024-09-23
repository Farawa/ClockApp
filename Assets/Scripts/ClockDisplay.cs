using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform hourArrow;
    [SerializeField] private RectTransform minuteArrow;
    [SerializeField] private RectTransform secondArrow;
    [SerializeField] private TextMeshProUGUI timeText;
    [Range(0.1f, 1f)]
    [SerializeField] private float clockMoveDuration = 0.5f;
    [SerializeField] private Ease easeType = Ease.Linear;

    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(() => Debug.Log(GetCurrentTimeFromArrows()));
    }

    public void SetTime(float hours, float minutes, float seconds)
    {
        RotateArrow(hourArrow, GetRotationFromValue(hours % 12 + minutes / 60, 12));
        RotateArrow(minuteArrow, GetRotationFromValue(minutes + seconds / 60, 60));
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

    public DateTime GetCurrentTimeFromArrows()
    {
        var now = DateTime.Now;
        var hour = (360 - hourArrow.eulerAngles.z) / 360 * 12;
        var minute = (360 - minuteArrow.eulerAngles.z) / 360 * 60;
        var seconds = (360 - secondArrow.eulerAngles.z) / 360 * 60;
        return new DateTime(now.Year, now.Month, now.Day, Mathf.RoundToInt(hour), Mathf.RoundToInt(minute), Mathf.RoundToInt(seconds));
    }
}
