using UnityEngine;
using Photon.VR;
using Photon.VR.Cosmetics;

public class PhotonRightHandCosmeticOnTouch : MonoBehaviour
{
    [Header("Target Object That Must Be Enabled")]
    public GameObject requiredEnabledObject;

    [Header("Tag That Can Trigger")]
    public string allowedTag = "Player";

    [Header("Right Hand Cosmetic Name")]
    public string rightHandCosmetic;

    private bool canTrigger = false;

    void Update()
    {
        // Check if required object is enabled in hierarchy
        if (requiredEnabledObject != null && requiredEnabledObject.activeInHierarchy)
        {
            canTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canTrigger) return;

        if (other.CompareTag(allowedTag))
        {
            GiveCosmetic();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canTrigger) return;

        if (collision.gameObject.CompareTag(allowedTag))
        {
            GiveCosmetic();
        }
    }

    void GiveCosmetic()
    {
        PhotonVRManager.SetCosmetic(CosmeticType.RightHand, rightHandCosmetic);
    }
}
