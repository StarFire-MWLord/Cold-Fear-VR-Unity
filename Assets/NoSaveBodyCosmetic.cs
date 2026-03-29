using UnityEngine;
using Photon.Pun;
using Photon.VR;
using Photon.VR.Cosmetics;

public class NoSaveBodyCosmetic : MonoBehaviourPunCallbacks
{
    [Header("Optional Default Body Cosmetic")]
    public string defaultBody;

    void Start()
    {
        ResetBody();
    }

    public override void OnJoinedRoom()
    {
        ResetBody();
    }

    void ResetBody()
    {
        // If empty → removes cosmetic
        if (string.IsNullOrEmpty(defaultBody))
        {
            PhotonVRManager.SetCosmetic(CosmeticType.Body, "");
        }
        else
        {
            PhotonVRManager.SetCosmetic(CosmeticType.Body, defaultBody);
        }
    }
}
