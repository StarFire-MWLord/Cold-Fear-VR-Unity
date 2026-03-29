using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SaberScript : MonoBehaviourPunCallbacks
{
    public GameObject[] PortalCosmetics;
    public GameObject Portal;
    public string HandTag;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(HandTag)) return;

        PhotonView handView = other.GetComponentInParent<PhotonView>();
        if (handView == null) return;

        photonView.RPC(nameof(RPC_PickupSaber), RpcTarget.AllBuffered, handView.Owner.ActorNumber);
    }

    [PunRPC]
    void RPC_PickupSaber(int actorNumber)
    {
        if (PortalCosmetics != null)
        {
            foreach (GameObject obj in PortalCosmetics)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }

        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            Portal.SetActive(false);
        }
    }
}