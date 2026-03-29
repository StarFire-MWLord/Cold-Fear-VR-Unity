using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

#if STEAMWORKS_NET
using Steamworks;
#endif

using PlayFab;
using PlayFab.ClientModels;

public class SetPhotonNameToSteamAndPlayFab : MonoBehaviourPunCallbacks
{
    void Start()
    {
        SetSteamName();
    }

    void SetSteamName()
    {
#if STEAMWORKS_NET
        if (SteamManager.Initialized)
        {
            string steamName = SteamFriends.GetPersonaName();
            PhotonNetwork.NickName = steamName;
            Debug.Log("Photon name set to Steam name: " + steamName);
        }
        else
        {
            Debug.LogWarning("Steam not initialized.");
        }
#else
        Debug.LogWarning("Steamworks.NET not detected.");
#endif
    }

    // Called when player joins a lobby
    public override void OnJoinedLobby()
    {
        SetPlayFabName();
    }

    void SetPlayFabName()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
        result =>
        {
            if (result.AccountInfo != null && result.AccountInfo.TitleInfo != null)
            {
                string playfabName = result.AccountInfo.TitleInfo.DisplayName;

                if (!string.IsNullOrEmpty(playfabName))
                {
                    PhotonNetwork.NickName = playfabName;
                    Debug.Log("Photon name set to PlayFab name: " + playfabName);
                }
            }
        },
        error =>
        {
            Debug.LogWarning("Failed to get PlayFab name: " + error.GenerateErrorReport());
        });
    }
}
