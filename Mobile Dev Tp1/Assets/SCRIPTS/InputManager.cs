using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Dictionary<string, float> axisValues = new Dictionary<string, float>();

    public void SetAxis(string axis, float value)
    {
        if (!axisValues.ContainsKey(axis))
            axisValues.Add(axis, value);

        axisValues[axis] = value;
    }

    public float GetAxis(string axis)
    {
#if UNITY_EDITOR
        return GetOrAddAxis(axis) + Input.GetAxis(axis);
#elif UNITY_ANDROID || UNITY_IOS
    return GetOrAddAxis(axis);
#elif UNITY_STANDALONE
    return Input.GetAxis(axis);
#endif
    }

    const float MIN_AXIS_VALUE = 0.75f;

    public bool IsUpPressed(string axis)
    {
        return GetAxis(axis) > MIN_AXIS_VALUE;
    }
    public bool IsDownPressed(string axis)
    {
        return GetAxis(axis) < -MIN_AXIS_VALUE;
    }

    private float GetOrAddAxis(string axis)
    {
        if (!axisValues.ContainsKey(axis))
            axisValues.Add(axis, 0f);

        return axisValues[axis];
    }
}
