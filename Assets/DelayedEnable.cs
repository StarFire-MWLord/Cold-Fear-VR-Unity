using System.Collections;
using UnityEngine;

public class DelayedEnable : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToEnable;
    public float delay = 5f;

    private Coroutine countdown;

    private void OnEnable()
    {
        // restart countdown every time enabled
        countdown = StartCoroutine(EnableAfterDelay());
    }

    private void OnDisable()
    {
        // stop countdown if disabled
        if (countdown != null)
        {
            StopCoroutine(countdown);
        }
    }

    IEnumerator EnableAfterDelay()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false); // ensure hidden first
        }

        yield return new WaitForSeconds(delay);

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }
}
