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
        if (isNeedMove == false || !ClockController.isCanMoveArrows) return;
        var screenCenter = (Vector2)transform.position;
        var a = Vector2.up;
        var b = ((Vector2)Input.mousePosition - screenCenter).normalized;
        decimal scalar = (decimal)(a.x * b.x + a.y * b.y);
        decimal modA = (decimal)(Mathf.Sqrt(a.x * a.x + a.y * a.y));
        decimal modB = (decimal)(Mathf.Sqrt(b.x * b.x + b.y * b.y));
        decimal cos = (decimal)((scalar / (modA * modB)));
        float angle = Mathf.Acos(float.Parse(cos.ToString())) * (180 / Mathf.PI);
        if (b.x < 0.0f)
        {
            angle = -angle;
            angle = angle + 360;
        }
        angle = 360 - angle;
        Debug.Log($"vA {a} vB {b} scalar {scalar} modA {modA} modB {modB} cos {cos} angle {angle}");
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isNeedMove = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isNeedMove = true;
    }
}
