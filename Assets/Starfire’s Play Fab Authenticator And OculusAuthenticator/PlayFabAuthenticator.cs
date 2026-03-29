using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;

public class PlayFabAuthenticator : MonoBehaviour
{
    [Header("THE SCRIPT WAS MADE BY STARFIRE")]

    private string playFabId;
    private string sessionTicket;
    [SerializeField] private GameObject playfabLogin;
    [SerializeField] private Playfablogin login;
    [SerializeField] private string playfabTitleId = "Your title ID";
    [SerializeField] private GameObject authenticationFailedObject; // Enable on failure
    [SerializeField] private GameObject disableOnFailureObject; // Disable on failure
    [SerializeField] private GameObject DisableOnfailure; // Disable on failure

    [SerializeField] private CustomIDRequester customIDRequester;

    void Start()
    {
        RequestCustomID();
    }

    private void RequestCustomID()
    {
        if (customIDRequester == null)
        {
            Debug.LogError("CustomIDRequester is not assigned!");
            SetAuthenticationState(false);
            return;
        }

        customIDRequester.RequestCustomID(OnCustomIDReceived);
    }

    private void OnCustomIDReceived(string customId)
    {
        if (string.IsNullOrEmpty(customId))
        {
            Debug.LogWarning("Failed to retrieve Custom ID. Authentication failed.");
            SetAuthenticationState(false);
            return;
        }

        AuthenticateUser(customId);
    }

    private void AuthenticateUser(string customId)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        playFabId = result.PlayFabId;
        sessionTicket = result.SessionTicket;
        Debug.Log($"Login Successful! PlayFabID: {playFabId}");
        SetAuthenticationState(true);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError($"Login Failed: {error.GenerateErrorReport()}");
        SetAuthenticationState(false);
    }

    private void SetAuthenticationState(bool isAuthenticated)
    {
        if (authenticationFailedObject != null)
            authenticationFailedObject.SetActive(!isAuthenticated); // Enable on failure

        if (disableOnFailureObject != null)
            disableOnFailureObject.SetActive(isAuthenticated); // Disable on failure

        if (DisableOnfailure != null)
            DisableOnfailure.SetActive(isAuthenticated); // Disable on failure

        if (login == null || playfabLogin == null)
        {
            Debug.Log("Playfab Is Missing! Disconnect From Photon!");
            PhotonNetwork.Disconnect();
        }


        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
                StartCoroutine(checkTitleID());
        }
        else
        {
                Debug.Log("Not connected to the internet!");
        }
    }

    private IEnumerator checkTitleID()
    {
        yield return new WaitForSeconds(5);
        if (PlayFabSettings.TitleId != playfabTitleId)
        {
            Debug.Log("Wrong title ID! Disconnect From Photon!");
            PhotonNetwork.Disconnect();
        }
    }
}