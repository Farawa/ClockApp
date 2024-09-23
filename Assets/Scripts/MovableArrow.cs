using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private bool isNeedMove = false;

    private void Start()
    {
        rectTransform = transform as RectTransform;
    }

    private void Update()
    {
        if (isNeedMove == false|| !ClockController.isCanMoveArrows) return;
        var screenCenter = new Vector2(Screen.width, Screen.height) / 2;
        var mousePos = (Vector2)Input.mousePosition - screenCenter;
        float angle = Vector3.Angle(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(mousePos.x, mousePos.y, 0.0f));
        if (mousePos.x < 0.0f)
        {
            angle = -angle;
            angle = angle + 360;
        }
        angle = 360-angle;
        Debug.Log(angle);
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("not move");
        isNeedMove = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("move");
        isNeedMove = true;
    }
}
