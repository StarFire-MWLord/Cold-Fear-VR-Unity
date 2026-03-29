using UnityEngine;
using Photon.Pun;
using Photon.VR;
using Photon.VR.Cosmetics;

public class NoSaveRightHand : MonoBehaviourPunCallbacks
{
    [Header("Optional Default Right Hand Cosmetic")]
    public string defaultRightHand;

    void Start()
    {
        ResetRightHand();
    }

    public override void OnJoinedRoom()
    {
        ResetRightHand();
    }

    void ResetRightHand()
    {
        // If empty → removes cosmetic
        if (string.IsNullOrEmpty(defaultRightHand))
        {
            PhotonVRManager.SetCosmetic(CosmeticType.RightHand, "");
        }
        else
        {
            PhotonVRManager.SetCosmetic(CosmeticType.RightHand, defaultRightHand);
        }
    }
}