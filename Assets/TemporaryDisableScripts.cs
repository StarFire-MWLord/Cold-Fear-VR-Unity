using System.Collections;
using UnityEngine;

public class TemporaryDisableScripts : MonoBehaviour
{
    [Header("Delay before scripts re-enable")]
    public float disableTime = 3f;

    [Header("Disable scripts in children too")]
    public bool includeChildren = true;

    private MonoBehaviour[] scripts;

    private void OnEnable()
    {
        if (includeChildren)
        {
            scripts = GetComponentsInChildren<MonoBehaviour>();
        }
        else
        {
            scripts = GetComponents<MonoBehaviour>();
        }

        StartCoroutine(DisableRoutine());
    }

    IEnumerator DisableRoutine()
    {
        // Disable all scripts except this one
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        yield return new WaitForSeconds(disableTime);

        // Re-enable scripts
        foreach (MonoBehaviour script in scripts)
        {
            if (script != null && script != this)
            {
                script.enabled = true;
            }
        }
    }
}
