using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using System.Collections;

public class VersionChecker : MonoBehaviour
{
    [Header("Script Created by StarFireVR")]
    [SerializeField] private string pastebinUrl;
    [Space]
    [SerializeField] private GameObject[] objectsToEnableIfOutdated;
    [SerializeField] private GameObject[] objectsToDisableIfOutdated;
    [Space]
    [SerializeField] private int currentVersion;
    [Space]
    [SerializeField] private bool shouldDisconnectFromPhoton;

    private const float CheckInterval = 30f;

    private void Start()
    {
        StartCoroutine(CheckForUpdates());
    }

    private IEnumerator CheckForUpdates()
    {
        while (true)
        {
            yield return StartCoroutine(CheckVersion());
            yield return new WaitForSeconds(CheckInterval);
        }
    }

    private IEnumerator CheckVersion()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(pastebinUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Version check failed: " + webRequest.error);
                yield break;
            }

            string latestVersionText = webRequest.downloadHandler.text;
            if (int.TryParse(latestVersionText, out int latestVersion) && latestVersion != currentVersion)
            {
                HandleOutdatedVersion();
            }
        }
    }

    private void HandleOutdatedVersion()
    {
        SetObjectsActiveState(objectsToEnableIfOutdated, true);
        SetObjectsActiveState(objectsToDisableIfOutdated, false);

        if (shouldDisconnectFromPhoton)
        {
            PhotonNetwork.Disconnect();
        }
    }

    private void SetObjectsActiveState(GameObject[] objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(isActive);
        }
    }
}
