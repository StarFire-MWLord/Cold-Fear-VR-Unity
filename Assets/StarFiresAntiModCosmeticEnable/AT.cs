using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AT: MonoBehaviour
{
    private bool checkForDisable = true;

    public AntiModCosmeticEnable objectToCheck;

    private bool hasDisconnected = false;

    void Update()
    {
        if (checkForDisable && objectToCheck != null)
        {
            if (!objectToCheck.gameObject.activeInHierarchy && !hasDisconnected)
            {
                Debug.LogWarning("Object has been disabled. Disconnecting from Photon...");
                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.Disconnect();
                }
                hasDisconnected = true;
            }
        }
}   }
