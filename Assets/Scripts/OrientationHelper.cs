using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationHelper : MonoBehaviour
{
    [Header("Landscape")]
    [SerializeField] private Vector2 anchorMinLandscape;
    [SerializeField] private Vector2 anchorMaxLandscape;
    [SerializeField] private Vector2 pivotLandscape;
    [SerializeField] private float width;
    [Header("Portrait")]
    [SerializeField] private Vector2 anchorMinPortrait;
    [SerializeField] private Vector2 anchorMaxPortrait;
    [SerializeField] private Vector2 pivotPortrait;
    [SerializeField] private float height;
    private ScreenOrientation lastOrientation = ScreenOrientation.Portrait;

    private void Start()
    {
        UpdateOrientation();
    }

    private void Update()
    {
        if (lastOrientation == Screen.orientation)
        {
            UpdateOrientation();
        }
    }

    private void UpdateOrientation()
    {
        lastOrientation = Screen.orientation;
        var rect = transform as RectTransform;
        bool isLandscape = lastOrientation == ScreenOrientation.LandscapeLeft || lastOrientation == ScreenOrientation.LandscapeRight;
        rect.anchorMin = isLandscape ? anchorMinLandscape : anchorMinPortrait;
        rect.anchorMax = isLandscape ? anchorMaxLandscape : anchorMaxPortrait;
        rect.pivot = isLandscape ? pivotLandscape : pivotPortrait;
        if (isLandscape) rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
        else rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
    }
}
