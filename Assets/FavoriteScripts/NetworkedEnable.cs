using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedEnable : MonoBehaviourPun
{
    public GameObject objectToEnable;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            photonView.RPC("EnableObject", RpcTarget.All);
        }
    }

    [PunRPC]
    public void EnableObject()
    {
        objectToEnable.SetActive(true);
    }
}
