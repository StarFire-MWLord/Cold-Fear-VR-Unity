using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NotconnectedBarrier : MonoBehaviour
{
    public GameObject Barrier;
    
    void Update()
    {
        if(PhotonNetwork.InRoom)
        {
            Barrier.SetActive(false);
        }
        else
        {
            Barrier.SetActive(true);
        }
    }
}
