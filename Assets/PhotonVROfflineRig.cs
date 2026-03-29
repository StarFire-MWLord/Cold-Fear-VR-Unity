using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.VR;

public class PhotonVROfflineRig : MonoBehaviourPunCallbacks
{
    [Header("made by Brikiscool1 for gorilla tag fangame")]
    public GameObject Rig;

    void Update()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            Rig.SetActive(false);
        }
        if (PhotonNetwork.IsConnected == false || PhotonNetwork.InRoom == false)
        {
            Rig.SetActive(true);
        }
    }
}
