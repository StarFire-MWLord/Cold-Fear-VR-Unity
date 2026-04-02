using UnityEngine;

public class AntiHandPhase : MonoBehaviour
{
    [Header("Script Created by StarFireVR")]
    [SerializeField] private Transform sphereTransform;
    [SerializeField] private Transform controllerTransform;

    private void FixedUpdate()
    {
        SyncRotations();
    }

    private void SyncRotations()
    {
        if (sphereTransform != null && controllerTransform != null)
        {
            sphereTransform.rotation = controllerTransform.rotation;
        }
    }
}
