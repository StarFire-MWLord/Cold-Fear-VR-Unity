using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class BanHammer : MonoBehaviour
{
    public PhotonView view;
    public int banLength = 168;

    private void OnTriggerEnter(Collider other)
    {
        // Only allow the OWNER of the hammer to attempt bans
        if (!view.IsMine) return;

        // Get target PlayFab ID (you NEED a script on player with this stored)
        PlayerPlayFabId targetIdScript = other.GetComponent<PlayerPlayFabId>();

        if (targetIdScript == null)
        {
            Debug.LogWarning("No PlayFab ID found on hit player.");
            return;
        }

        string targetPlayFabId = targetIdScript.playFabId;
        string hammerOwnerId = PlayFabSettings.staticPlayer.PlayFabId;

        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "secureBanHammer",
            FunctionParameter = new
            {
                targetId = targetPlayFabId,
                ownerId = hammerOwnerId,
                duration = banLength,
                reason = "Got hit with the ban hammer!"
            }
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnBanSuccess, OnBanFail);
    }

    private void OnBanSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("CloudScript Result: " + result.FunctionResult);
    }

    private void OnBanFail(PlayFabError error)
    {
        Debug.LogError("Ban Failed: " + error.ErrorMessage);
    }
}