using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabNameSystem : MonoBehaviour
{
    [Header("Player Name Text")]
    public TMP_Text playerNameText; // Drag the player's name text here

    public string namePrefix = "Subject-";

    void Start()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Login();
        }
        else
        {
            GetDisplayName();
        }
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        GetDisplayName();
    }

    void GetDisplayName()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            string currentName = result.AccountInfo.TitleInfo.DisplayName;

            if (string.IsNullOrEmpty(currentName))
            {
                GenerateAndSetName();
            }
            else
            {
                SetPlayerName(currentName);
            }

        }, OnError);
    }

    void GenerateAndSetName()
    {
        int randomNumbers = Random.Range(1000, 9999);
        string generatedName = namePrefix + randomNumbers;

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = generatedName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result =>
        {
            SetPlayerName(generatedName);
        }, OnError);
    }

    void SetPlayerName(string name)
    {
        if (playerNameText != null)
        {
            playerNameText.text = name;
        }

        Debug.Log("Player Name: " + name);
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}