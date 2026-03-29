using UnityEngine;
using Photon.Pun;

public class AutoPhotonName : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        CheckAndSetName();
    }

    private void Update()
    {
        // Constantly check (infinite auto-detect)
        if (string.IsNullOrWhiteSpace(PhotonNetwork.NickName))
        {
            SetRandomName();
        }
    }

    void CheckAndSetName()
    {
        if (string.IsNullOrWhiteSpace(PhotonNetwork.NickName))
        {
            SetRandomName();
        }
    }

    void SetRandomName()
    {
        int randomNumber = Random.Range(1000, 9999); // 4-digit number
        PhotonNetwork.NickName = "Gorilla-" + randomNumber;

        Debug.Log("Assigned Photon Name: " + PhotonNetwork.NickName);
    }

    // Also check when joining lobby
    public override void OnJoinedLobby()
    {
        CheckAndSetName();
    }

    // Also check when joining room
    public override void OnJoinedRoom()
    {
        CheckAndSetName();
    }
}