using UnityEngine;
using Photon.Pun;
using Photon.VR;
using Photon.VR.Cosmetics;

public class RightHandCosmeticOnTouch : MonoBehaviourPun
{
    [Header("Tag that activates cosmetic")]
    public string triggerTag = "HandTag";

    [Header("Right Hand Cosmetic Name")]
    public string rightHandCosmetic;

    private bool given = false;

    private void OnTriggerEnter(Collider other)
    {
        if (given) return;

        if (other.CompareTag(triggerTag))
        {
            given = true;
            photonView.RPC("GiveCosmetic", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void GiveCosmetic()
    {
        PhotonVRManager.SetCosmetic(CosmeticType.RightHand, rightHandCosmetic);
    }
}
