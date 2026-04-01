using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GorillaLocomotion;

public class BetterTeleportMonke : MonoBehaviour
{
    [Header("THIS NEEDS TO BE CALLED BY A SCRIPT OR AN EVENT")]
    public Transform newPos;
    public Transform gorillaPos;
    private Player gorillaMovement;
    void Start()
    {
        gorillaPos.gameObject.TryGetComponent<Player>(out gorillaMovement);
    }
    public void Teleport()
    {
        if (gorillaMovement != null)
        {
            gorillaMovement.locomotionEnabledLayers = 0;

            // Reset Rigidbody velocity using the new properties
            Rigidbody rb = gorillaPos.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;      // replaces velocity
                rb.angularVelocity = Vector3.zero;     // stays the same
            }

            gorillaPos.position = newPos.position;

            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        gorillaMovement.locomotionEnabledLayers = 1;
    }
}
