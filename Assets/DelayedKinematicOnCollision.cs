using System.Collections;
using UnityEngine;

public class DelayedKinematicOnCollision : MonoBehaviour
{
    [Header("Target Settings")]
    public GameObject targetObject; // Drag the specific player object here

    [Header("Timing Settings")]
    public float delayBeforeFreeze = 1f; // Seconds before enabling kinematic
    public float freezeDuration = 3f;    // Seconds to keep it kinematic

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the target and has "Player" tag
        if (collision.gameObject != null && collision.gameObject == targetObject && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                StartCoroutine(SetKinematicAfterDelay(rb));
            }
        }
    }

    private IEnumerator SetKinematicAfterDelay(Rigidbody rb)
    {
        // Wait for delay before freezing
        yield return new WaitForSeconds(delayBeforeFreeze);

        // Enable kinematic to freeze physics
        rb.isKinematic = true;

        // Wait for freeze duration
        yield return new WaitForSeconds(freezeDuration);

        // Disable kinematic to unfreeze physics
        rb.isKinematic = false;
    }
}