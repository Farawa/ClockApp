using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputTime : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ClockController clockController;
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(SetTime);
    }

    private void SetTime()
    {
        try
        {
            DateTime now = DateTime.Now;
            var times = inputField.text.Split(":");
            var hour = float.Parse(times[0]);
            var minute = float.Parse(times[1]);
            var seconds = float.Parse(times[2]);
            clockController.SetTime(new DateTime(now.Year, now.Month, now.Day, Mathf.RoundToInt(hour), Mathf.RoundToInt(minute), Mathf.RoundToInt(seconds)));
            inputField.text = "";
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "time is set";
        }
        catch
        {
            inputField.text = "";
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "error format or value";
        }
    }
}
