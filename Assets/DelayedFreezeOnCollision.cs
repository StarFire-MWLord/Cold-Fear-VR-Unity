using System.Collections;
using UnityEngine;

public class DelayedFreezeOnCollision : MonoBehaviour
{
    [Header("Target Settings")]
    public GameObject targetObject; // Drag the specific player object here

    [Header("Freeze Settings")]
    public float delayBeforeFreeze = 1f; // Seconds before freezing
    public float freezeDuration = 3f;    // Seconds to freeze

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding is the target and has "Player" tag
        if (collision.gameObject != null && collision.gameObject == targetObject && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FreezeAfterDelay(collision.gameObject));
        }
    }

    private IEnumerator FreezeAfterDelay(GameObject player)
    {
        // Wait for delay before freezing
        yield return new WaitForSeconds(delayBeforeFreeze);

        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Freeze all movement and rotation
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // Wait for freeze duration
        yield return new WaitForSeconds(freezeDuration);

        if (rb != null)
        {
            // Unfreeze movement and rotation
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}