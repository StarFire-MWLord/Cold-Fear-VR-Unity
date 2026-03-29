using UnityEngine;

public class RandomEnable : MonoBehaviour
{
    // The GameObject you want to randomly enable
    public GameObject targetObject;

    // Time interval in seconds (60 minutes = 3600 seconds)
    private float timeInterval = 3600f;

    // Time since the last enable
    private float timeSinceLastEnable = 0f;

    // Randomness factor for testing (this can be used to randomly trigger enablement within the script)
    public bool randomTestEnable = false;

    void Update()
    {
        // If you want to randomly enable the object to test it
        if (randomTestEnable)
        {
            RandomEnableObject();
            randomTestEnable = false; // Reset test mode after one enable
        }

        // Update the time since last enable
        timeSinceLastEnable += Time.deltaTime;

        // If 60 minutes have passed (3600 seconds), randomly enable the target object
        if (timeSinceLastEnable >= timeInterval)
        {
            RandomEnableObject();
            timeSinceLastEnable = 0f; // Reset the timer after enabling
        }
    }

    // Function to randomly enable or disable the target object
    private void RandomEnableObject()
    {
        if (targetObject != null)
        {
            bool enable = Random.value > 0.5f;  // 50% chance to enable or disable
            targetObject.SetActive(enable);
            Debug.Log("Object is " + (enable ? "enabled" : "disabled"));
        }
        else
        {
            Debug.LogWarning("Target Object is not assigned.");
        }
    }
}