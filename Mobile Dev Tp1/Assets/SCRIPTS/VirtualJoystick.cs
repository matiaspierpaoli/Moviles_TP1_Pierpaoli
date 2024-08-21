using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private RectTransform stick = null;
    [SerializeField] private Image background = null;

    public string playerID = "";
    public float limit = 180f;

    public string horizontalInputName = "Horizontal";
    public string verticalInputName = "Vertical";

    private void OnDisable()
    {
        SetHorizontal(0);
        SetVertical(0);
    }

    private void SetHorizontal(float value)
    {
        inputManager.SetAxis(horizontalInputName + playerID, value);
    }
    
    private void SetVertical(float value)
    {
        inputManager.SetAxis(verticalInputName + playerID, value);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 pos = ConvertToLocal(eventData);
        if (pos.magnitude > limit)
            pos = pos.normalized * limit;
        stick.anchoredPosition = pos;

        float x = pos.x / limit;
        float y = pos.y / limit;

        SetHorizontal(x);
        SetVertical(y);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        background.color = Color.gray;
        stick.anchoredPosition = Vector2.zero;
        SetHorizontal(0);
        SetVertical(0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.color = Color.red;
        stick.anchoredPosition = ConvertToLocal(eventData);
    }

    private Vector2 ConvertToLocal(PointerEventData eventData)
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        transform as RectTransform,
        eventData.position,
        eventData.enterEventCamera,
        out newPos);
        return newPos;
    }
}
