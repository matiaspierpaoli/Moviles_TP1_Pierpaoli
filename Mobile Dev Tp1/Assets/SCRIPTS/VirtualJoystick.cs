using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform stick = null;
    [SerializeField] private Image background = null;

    public int playerID = -1;
    public float limit = 180f;

    public string horizontalInputName = "Horizontal";
    public string verticalInputName = "Vertical";

    private void OnEnable()
    {
    #if UNITY_STANDALONE
            gameObject.SetActive(false);  // Disable Virtual Joystick on PC
    #endif
    }

    private void OnDisable()
    {
        if (InputManager.Instance!= null)
        {
            SetHorizontal(0);
            SetVertical(0);
        }
    }

    private void SetHorizontal(float value)
    {
        InputManager.Instance.SetAxis(horizontalInputName + playerID.ToString(), value);
    }

    private void SetVertical(float value)
    {
        InputManager.Instance.SetAxis(verticalInputName + playerID.ToString(), value);
    }

    public void OnDrag(PointerEventData eventData)
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

    public void OnPointerUp(PointerEventData eventData)
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
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.enterEventCamera,
            out Vector2 newPos);
        return newPos;
    }
}
