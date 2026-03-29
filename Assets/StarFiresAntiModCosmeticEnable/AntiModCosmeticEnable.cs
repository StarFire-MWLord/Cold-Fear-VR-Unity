using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.VR;

public class AntiModCosmeticEnable : MonoBehaviour
{
    private bool checkForEnable = true;

    [Tooltip("The ModCosmeticObjects To Monitor.")]
    [Space]
    public List<GameObject> ModCosmeticObjectsToCheck;

    private bool hasDisconnected = false;

    void Update()
    {
        if (checkForEnable && ModCosmeticObjectsToCheck != null && !hasDisconnected)
        {
            foreach (GameObject obj in ModCosmeticObjectsToCheck)
            {
                if (obj != null && obj.activeInHierarchy)
                {
                    Debug.LogWarning($"Detected enabled object: {obj.name}. Disconnecting from Photon...");
                    if (PhotonNetwork.IsConnected)
                    {
                        PhotonNetwork.Disconnect();
                    }
                    hasDisconnected = true;
                    break;
                }
            }
        }
    }
}
