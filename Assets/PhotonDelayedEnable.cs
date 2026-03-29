using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PhotonDelayedEnable : MonoBehaviourPun
{
    [Header("Object To Enable")]
    public GameObject objectToEnable;

    [Header("Delay In Seconds")]
    public float delay = 5f;

    private bool started = false;

    void Start()
    {
        // Only Master starts it so it doesn't run multiple times
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPC_StartCountdown", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void RPC_StartCountdown()
    {
        if (!started)
        {
            started = true;
            StartCoroutine(EnableAfterDelay());
        }
    }

    IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }
}
