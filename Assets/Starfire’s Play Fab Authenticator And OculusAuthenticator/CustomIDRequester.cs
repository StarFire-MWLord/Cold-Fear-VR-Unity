using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;

public class CustomIDRequester : MonoBehaviour
{
    public void RequestCustomID(Action<string> onCustomIDReceived)
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), 
            result =>
            {
                string customId = ExtractCustomID(result);
                onCustomIDReceived?.Invoke(customId);
            },
            error =>
            {
                Debug.LogError($"Failed to retrieve Custom ID: {error.GenerateErrorReport()}");
                onCustomIDReceived?.Invoke(null);
            });
    }

    private string ExtractCustomID(GetAccountInfoResult result)
    {
        if (result.AccountInfo?.CustomIdInfo?.CustomId != null)
        {
            return result.AccountInfo.CustomIdInfo.CustomId;
        }
        
        Debug.LogWarning("No Custom ID found for the user.");
        return null;
    }
}