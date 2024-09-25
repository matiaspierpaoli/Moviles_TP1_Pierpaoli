using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    Dictionary<string, float> axisValues = new Dictionary<string, float>();

    #if UNITY_ANDROID || UNITY_IOS
        const float MIN_AXIS_VALUE = 0.75f;
    #elif UNITY_STANDALONE
        const float MIN_AXIS_VALUE = 0.1f;
        const string PLATFORM_TYPE = "_PC";
    #endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public float GetAxis(string axis, string player)
    {
    #if UNITY_ANDROID || UNITY_IOS
            return GetOrAddAxis(axis + player);
    #elif UNITY_STANDALONE
            return Input.GetAxis(axis + PLATFORM_TYPE + "_" + player); 
    #endif
    }

    #if UNITY_ANDROID || UNITY_IOS
    private float GetOrAddAxis(string axis)
    {
        if (!axisValues.ContainsKey(axis))
            axisValues.Add(axis, 0f);

        return axisValues[axis];
    }
    #endif

    public bool IsUpPressed(string axis, string player)
    {
        return GetAxis(axis, player) > MIN_AXIS_VALUE;
    }

    public bool IsDownPressed(string axis, string player)
    {
        return GetAxis(axis, player) < -MIN_AXIS_VALUE;
    }

    public float GetMinAxisValue()
    {
        return MIN_AXIS_VALUE;
    }

    public void SetAxis(string axis, float value)
    {
        if (!axisValues.ContainsKey(axis))
            axisValues.Add(axis, value);

        axisValues[axis] = value;
    }
}