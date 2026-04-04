using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.VR;
using Photon.VR.Cosmetics;
using PlayFab;
using PlayFab.ClientModels;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StarFiresModCosmeticHandler : MonoBehaviour
{
    [Header("This Script Was Made By StarFireVR")]
    [Space]
    [Header("Cosmetic Information")]
    [SerializeField] private CosmeticType cosmeticType;
    [SerializeField] private string cosmeticName;
    [SerializeField] private string playfabItemId;
    [SerializeField] private string catalogName;
    [Space]
    [Header("Tag for Hand Detection")]
    [SerializeField] private string[] triggerTags = { "HandTag" };
    [Header("Options")]
    [Space]
    [Header("Discord Webhook Settings")]
    [Space]
    public bool sendToDiscordWebhook = true;
    [Space]
    public string discordWebhookURL = "PASTE_YOUR_WEBHOOK_URL_HERE";
    [TextArea(3, 10)] public string customDiscordMessage = "";
    [Space]
    private string playerPlayFabID = "Unknown";
    [Space]
    [Header("Game Settings")]
    [SerializeField] private bool quitIfNotOwned = false;

    private void Start()
    {
        StartCoroutine(WaitForPlayFabID());
    }

    private IEnumerator WaitForPlayFabID()
    {
        float timeout = 5f;
        float timer = 0f;

        while (string.IsNullOrEmpty(PlayFabSettings.staticPlayer.PlayFabId) && timer < timeout)
        {
            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
        }

        if (!string.IsNullOrEmpty(PlayFabSettings.staticPlayer.PlayFabId))
        {
            playerPlayFabID = PlayFabSettings.staticPlayer.PlayFabId;
            Debug.Log("[StarFiresModCosmeticHandler] PlayFab ID acquired: " + playerPlayFabID);
        }
        else
        {
            Debug.LogWarning("[StarFiresModCosmeticHandler] Failed to get PlayFab ID within 5 seconds. Defaulting to 'Unknown'.");
            playerPlayFabID = "Unknown";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsHandDetected(other))
        {
            StartCoroutine(CheckAndApplyCosmetic());
        }
    }

    private bool IsHandDetected(Collider collider)
    {
        foreach (string tag in triggerTags)
        {
            if (collider.CompareTag(tag))
                return true;
        }
        return false;
    }

    private IEnumerator CheckAndApplyCosmetic()
    {
        bool isChecked = false;
        bool ownsItem = false;

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            foreach (var item in result.Inventory)
            {
                if (item.ItemId == playfabItemId && item.CatalogVersion == catalogName)
                {
                    ownsItem = true;
                    break;
                }
            }
            isChecked = true;
        }, error =>
        {
            Debug.LogError($"PlayFab Inventory Check Failed: {error.GenerateErrorReport()}");
            isChecked = true;
        });

        while (!isChecked)
        {
            yield return null;
        }

        if (ownsItem)
        {
            Debug.Log($"Applying {cosmeticName} ({cosmeticType}) to the avatar.");
            PhotonVRManager.SetCosmetic(cosmeticType, cosmeticName);
        }
        else
        {
            Debug.LogWarning($"Player does not own {cosmeticName}. Cosmetic not applied.");

            if (sendToDiscordWebhook)
            {
                string finalMessage = $"{customDiscordMessage}\n\n🔹 **PlayFab ID:** `{playerPlayFabID}`";
                StartCoroutine(SendDiscordWebhook(finalMessage));
            }

            if (quitIfNotOwned)
            {
                StartCoroutine(QuitAfterDelay(2f));
            }
        }
    }

    private IEnumerator SendDiscordWebhook(string message)
    {
        if (string.IsNullOrWhiteSpace(discordWebhookURL))
        {
            Debug.LogError("[StarFiresModCosmeticHandler] Webhook URL is missing!");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("content", message);

        using (UnityWebRequest request = UnityWebRequest.Post(discordWebhookURL, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[StarFiresModCosmeticHandler] Failed to send Discord webhook: " + request.error);
            }
            else
            {
                Debug.Log("[StarFiresModCosmeticHandler] Successfully sent Discord webhook notification.");
            }
        }
    }

    private IEnumerator QuitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Quitting game because cosmetic is not owned.");
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(StarFiresModCosmeticHandler))]
public class StarFiresModCosmeticHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "*****************************************************\r\n" +
            " * COPYRIGHT (C) 2025 STARFIREVR STUDIOS. ALL RIGHTS RESERVED.\r\n" +
            " * \r\n" +
            " * THIS CODE AND ITS CONTENTS ARE THE PROPERTY OF STARFIREVR STUDIOS.\r\n" +
            " * DO NOT REPRODUCE, DISTRIBUTE, MODIFY, OR USE THIS CODE WITHOUT \r\n" +
            " * EXPLICIT PERMISSION. ANY UNAUTHORIZED USE WILL RESULT IN POTENTIAL\r\n" +
            " * LEGAL ACTION. THIS SCRIPT IS COPYRIGHTED AND CANNOT BE \r\n" +
            " * CLAIMED, LEAKED, OR SHARED WITHOUT CONSENT.\r\n" +
            " * \r\n" +
            " * FOR LICENSE INFORMATION, PLEASE CONTACT STARFIREVR STUDIOS.\r\n" +
            "*****************************************************",
            MessageType.Info
        );

        DrawDefaultInspector();
    }
}
#endif