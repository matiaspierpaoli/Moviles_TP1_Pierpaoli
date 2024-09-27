using System.Collections;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject objectToToggle;
    public float toggleInterval = 1f;

    private Coroutine toggleCoroutine;

    void Start()
    {
        if (objectToToggle != null)
        {
            toggleCoroutine = StartCoroutine(ToggleObjectCoroutine());
        }
    }

    private IEnumerator ToggleObjectCoroutine()
    {
        while (true)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
            yield return new WaitForSeconds(toggleInterval);
        }
    }

    void OnDisable()
    {
        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
        }
    }
}
