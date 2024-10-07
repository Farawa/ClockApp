using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputTime : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ClockController clockController;
    [SerializeField] private Button button;

    public void SetButtonAction(Action callback)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback?.Invoke());
    }

    public bool TrySetTime(out DateTime time)
    {
        try
        {
            DateTime now = DateTime.Now;
            var times = inputField.text.Split(":");
            var hour = float.Parse(times[0]);
            var minute = float.Parse(times[1]);
            var seconds = float.Parse(times[2]);
            time = new DateTime(now.Year, now.Month, now.Day, Mathf.RoundToInt(hour), Mathf.RoundToInt(minute), Mathf.RoundToInt(seconds));
            inputField.text = "";
            return true;
        }
        catch
        {
            inputField.text = "";
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "error format or value";
            time = DateTime.Now;
            return false;
        }
    }
}
