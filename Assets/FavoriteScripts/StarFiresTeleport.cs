using UnityEngine;

public class StarFiresTeleport : MonoBehaviour
{
    [Header("This Script Was Made By StarFireVR")]
    [Space]
    [SerializeField] private GameObject playerToTeleport;
    [SerializeField] private Transform destinationPoint;
    private string MainCamera = "MainCamera";
    private string HandTag = "HandTag";

    [Header("Colliders to Disable During Teleport")]
    [SerializeField] private GameObject[] collidersToDisable;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (IsHandCollider(other))
        {
            Debug.Log("Hand detected. Starting teleport.");
            StartCoroutine(TeleportPlayer());
        }
        else
        {
            Debug.Log("Collider tag does not match hand tags.");
        }
    }

    private bool IsHandCollider(Collider col)
    {
        return col.CompareTag(MainCamera) || col.CompareTag(HandTag);
    }

    private System.Collections.IEnumerator TeleportPlayer()
    {
        if (playerToTeleport == null || destinationPoint == null)
        {
            Debug.LogError("Teleport failed: Player or destination not assigned.");
            yield break;
        }

        Rigidbody playerRb = playerToTeleport.GetComponent<Rigidbody>();

        if (playerRb != null)
        {
            playerRb.isKinematic = true;
        }

        if (collidersToDisable != null)
        {
            foreach (var target in collidersToDisable)
            {
                SetCollidersEnabled(target, false);
            }
        }

        yield return new WaitForSeconds(0.09f);

        CharacterController cc = playerToTeleport.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            playerToTeleport.transform.position = destinationPoint.position;
            cc.enabled = true;
        }
        else
        {
            playerToTeleport.transform.position = destinationPoint.position;
        }

        yield return new WaitForSeconds(0.1f);

        if (collidersToDisable != null)
        {
            foreach (var target in collidersToDisable)
            {
                SetCollidersEnabled(target, true);
            }
        }

        if (playerRb != null)
        {
            playerRb.isKinematic = false;
        }
    }

    private void SetCollidersEnabled(GameObject target, bool enabledState)
    {
        Collider[] colliders = target.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = enabledState;
        }
    }
}
