using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedDisable : MonoBehaviourPun
{
    public GameObject objectToDisable;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            photonView.RPC("DisableObject", RpcTarget.All);
        }
    }

    [PunRPC]
    public void DisableObject()
    {
        objectToDisable.SetActive(false);
    }
}
