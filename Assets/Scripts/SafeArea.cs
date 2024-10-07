using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea;
    private ScreenOrientation lastOrientation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (lastSafeArea != Screen.safeArea||Screen.orientation!= lastOrientation)
        {
            lastSafeArea = Screen.safeArea;
            lastOrientation = Screen.orientation;
            Refresh();
        }
    }

    public void Refresh()
    {
        var anchorMin = lastSafeArea.position;
        var anchorMax = lastSafeArea.position + lastSafeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
