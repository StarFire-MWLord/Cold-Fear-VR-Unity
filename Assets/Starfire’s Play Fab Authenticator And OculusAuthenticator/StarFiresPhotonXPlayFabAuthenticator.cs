using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class StarFiresPhotonXPlayFabAuthenticator : MonoBehaviourPunCallbacks
{
    [Header("This Script Was Made By StarFireVR")]
    [Space]
    public string PlayFabTitleId = "YOUR_PLAYFAB_TITLE_ID";
    private string CustomId;

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = PlayFabTitleId;

        if (PlayerPrefs.HasKey("SavedPlayFabId"))
        {
            CustomId = PlayerPrefs.GetString("SavedPlayFabId");
            Debug.Log("Logging in with existing PlayFab ID as CustomId: " + CustomId);
            LoginWithCustomID();
        }
        else
        {
            Debug.LogError("No saved PlayFab ID found! Cannot log in.");
        }
    }

    private void LoginWithCustomID()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = CustomId,
            CreateAccount = false,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserAccountInfo = true
            }
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFab login successful! PlayFabId: " + result.PlayFabId);

        GetPhotonAuthenticationToken();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("PlayFab login failed: " + error.GenerateErrorReport());

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    private void GetPhotonAuthenticationToken()
    {
        var request = new GetPhotonAuthenticationTokenRequest
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
        };

        PlayFabClientAPI.GetPhotonAuthenticationToken(request, OnPhotonAuthTokenReceived, OnPhotonAuthTokenFailed);
    }

    private void OnPhotonAuthTokenReceived(GetPhotonAuthenticationTokenResult result)
    {
        Debug.Log("Photon Auth Token Received.");

        AuthenticationValues authValues = new AuthenticationValues
        {
            AuthType = CustomAuthenticationType.Custom
        };

        authValues.AddAuthParameter("username", PlayFabSettings.staticPlayer.PlayFabId);
        authValues.AddAuthParameter("token", result.PhotonCustomAuthenticationToken);

        PhotonNetwork.AuthValues = authValues;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnPhotonAuthTokenFailed(PlayFabError error)
    {
        Debug.LogError("Photon Auth Token failed: " + error.GenerateErrorReport());
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server using PlayFab credentials.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Disconnected from Photon: " + cause.ToString());
    }

    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.LogError("Photon custom authentication failed: " + debugMessage);
        PhotonNetwork.Disconnect();
    }
}