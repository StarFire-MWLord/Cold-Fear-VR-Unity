using System.Collections;
using UnityEngine;
using Photon.VR;
using Photon.VR.Cosmetics;

public class CosmeticHandler : MonoBehaviour
{
    [Header("This Script Was Made By StarFireVR")]
    [Space]
    [Header("Cosmetic Information")]
    [SerializeField] private CosmeticType cosmeticType;
    [SerializeField] private string cosmeticName;
    [Space]
    [Header("Tag for Hand Detection")]
    [SerializeField] private string handTag = "HandTag";

    private void OnTriggerEnter(Collider other)
    {
        if (IsHandDetected(other))
        {
            ApplyCosmetic();
        }
    }

    private bool IsHandDetected(Collider collider)
    {
        return collider.CompareTag(handTag);
    }

    private void ApplyCosmetic()
    {
        Debug.Log($"Applying {cosmeticName} ({cosmeticType}) to the avatar.");
        PhotonVRManager.SetCosmetic(cosmeticType, cosmeticName);
    }
}
