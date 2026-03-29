using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.VR;
using Photon.VR.Cosmetics;

public class ChangeRightCosmetic : MonoBehaviour
{
    public string Right;

    private bool canTrigger = false;

    void Start()
    {
        // Small delay so it doesn't trigger instantly on spawn
        StartCoroutine(EnableTriggerDelay());
    }

    IEnumerator EnableTriggerDelay()
    {
        yield return new WaitForSeconds(1f); // you can change this delay
        canTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canTrigger) return;

        if (other.CompareTag("HandTag"))
        {
            PhotonVRManager.SetCosmetic(CosmeticType.RightHand, Right);
        }
    }
}