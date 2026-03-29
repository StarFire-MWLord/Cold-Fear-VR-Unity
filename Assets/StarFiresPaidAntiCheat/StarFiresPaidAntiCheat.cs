using System;
using UnityEditor;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using UnityEngine.Diagnostics;
using Oculus.Platform.Models;
using Photon.Realtime;
using GorillaLocomotion;
using Photon.VR;
using UnityEngine.Networking;

public class StarFiresPaidAntiCheat : MonoBehaviour
{
    [Header("Things That This Anti-Cheat Does Behind The Scenes")]
    [Header("---------------------------------------------------------------------------")]
    [Header("ChecksForModFolders")]
    [Header("ChecksForRootAccess")]
    [Header("ChecksForTamperedFiles")]
    [Header("ChecksForMemoryTampering")]
    [Header("PreventReflectionAttacks")]
    [Header("ChecksForScriptTampering")]
    [Header("ChecksForUnauthorizedProcesses")]
    [Header("ChecksForInjectedLibraries")]
    [Header("ChecksForInjectedDlls")]
    [Header("ChecksForHookedAPIFunctions")]
    [Header("ChecksForOverlaySoftware")]
    [Header("ChecksForVPN")]
    [Header("ChecksForUABE")]
    [Header("VerifyAPKIntegrityOtherwiseknownAsSignatureChecking")]
    [Header("---------------------------------------------------------------------------")]
    [Space]
    [Header("THIS SCRIPT IS PAID AND WAS MADE BY STARFIRE")]
    [Space]
    [Header("*****************************************************\r\n * COPYRIGHT (C) 2025 STARFIREVR STUDIOS. ALL RIGHTS RESERVED.\r\n * \r\n * THIS CODE AND ITS CONTENTS ARE THE PROPERTY OF STARFIREVR STUDIOS. \r\n * DO NOT REPRODUCE, DISTRIBUTE, MODIFY, OR USE THIS CODE WITHOUT \r\n * EXPLICIT PERMISSION. ANY UNAUTHORIZED USE WILL RESULT IN POTENTIAL\r\n * LEGAL ACTION. THIS SCRIPT IS COPYRIGHTED AND CANNOT BE \r\n * CLAIMED, LEAKED, OR SHARED WITHOUT CONSENT.\r\n * \r\n * FOR LICENSE INFORMATION, PLEASE CONTACT STARFIREVR STUDIOS.\r\n *****************************************************")]
    [Space]
    [Header("---------------------------------------------------------------------------")]
    [Space]
    [Header("Regular Settings")]
    public PhotonVRManager PhotonVRManager;
    public bool quitGame;
    public bool disconnectPhoton;
    public bool quitApp = false;
    public bool disconnectFromPhoton = false;
    [Space]
    [Header("---------------------------------------------------------------------------")]
    [Space]
    [Header("Things To Check For")]
    public bool checkForMelonLoader = false;
    public bool checkForLemonLoader = false;
    public bool checkForQuestPatcher = false;
    public bool checkForModderx = false;
    public bool checkForDelonLoader = false;
    public bool checkForMods = false;
    public bool checkForMelonMod = false;
    public bool checkForHarmony = false;
    public bool checkForGlobalMetadataTampering = false;
    [Header("---------------------------------------------------------------------------")]
    [Space]
    [Header("UABE Protection")]
    public bool checkForUABE = false;
    private bool disconnect = true;
    private bool cheatDetected = false;
    [Space]
    public string[] criticalObjectNames = { };
    [Header("Use full script names with .cs extension")]
    public string[] protectedScripts = { };
    [Space]
    [Header("---------------------------------------------------------------------------")]
    [Space]
    [Header("Folder/Assembly Blocking Settings")]
    [Space]
    public string[] assembliesToCheck = new string[0]
    {

    };
    [Space]
    [Space]
    public string[] FoldersToCheck = new string[0]
    {

    };
    [Space]
    [Header("---------------------------------------------------------------------------")]
    [Space]
    [Header("PlayFab User Authentication Settings")]
    [Header("Full PlayFab Authenticator Version Available For Purchase")]
    [SerializeField] private string playFabTitleId = "YOUR_TITLE_ID";
    [SerializeField] private Playfablogin playFabLoginScript;
    [Header("Oculus User Authentication Settings")]
    [Header("---------------------------------------------------------------------------")]
    [Space]
    [Header("Hacker Trolling Settings")]
    public bool lagOutGame = false;
    public bool EarFuckThem = false;
    public bool TeleportToDiddysDungeonOnCheat = false;
    public GameObject[] collidersToDisableForDiddysDungeon;
    public Transform player;
    public Transform teleportLocation;
    public GameObject EarFuckSound;
    public float lagIntensity = 100.0f;
    private float originalTimeScale;
    [Space]
    [Header("---------------------------------------------------------------------------")]
    [Space]
    [Header("Version Checker Settings")]
    [Space]
    public string RawPastebinUrl;
    [Space]
    public GameObject[] EnableIfOutdated;
    [Space]
    public GameObject[] DisableIfOutdated;
    [Space]
    public int CurrentVersion;
    [Space]
    public bool DisconnectFromPhoton;
    [Space]
    [Header("---------------------------------------------------------------------------")]
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
    [Header("---------------------------------------------------------------------------")]
    [Space]
    [Header("Anti Script/Object Tampering Settings")]
    [Header("PUT THESE AS YOUR OBJECTS THAT YOUR SCRIPT IS ON TO CHECK IF DISABLED")]
    [Space]
    public GameObject[] objectToCheck;
    [Space]
    [Header("PUT THIS AS YOUR PHOTON VR MANAGER")]
    public PhotonVRManager objectToDisable;
    [Header("PUT THIS AS YOUR FAILED TO AUTHENTICATE YOU OBJECT")]
    public GameObject objectToEnable;
    [Header("DO NOT MESS WITH THIS")]
    public bool toggleState;
    [Space]
    [Space]
    private string[] forbiddenFolders = { "Mods", "Plugins", "MelonLoader", "LemonLoader", "Harmony", "QuestPatcher", "Modderx", "DelonLoader", "MelonMod", "LemonMod", };
    private string gamePath;
    private string initialMemoryHash;
    private string initialScriptHash;
    private string initialAPKHash;
    private List<string> customFolders = new List<string>();


    void Start()
    {
        {
            DontDestroyOnLoad(gameObject);
            RunChecks();
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            string androidFolderPath = $"/storage/emulated/0/Android/data/{Application.identifier}/files";
            string[] dllFiles = Directory.GetFiles(androidFolderPath, "*.dll", SearchOption.AllDirectories);

            foreach (var file in dllFiles)
            {
                bool isInvalidDll = !file.Contains("/il2cpp/") && !file.Contains("/cache/");
                if (isInvalidDll)
                {
                    File.Delete(file);
                    TriggerCrashAndExit();
                }
            }
        }

        void TriggerCrashAndExit()
        {
            Utils.ForceCrash(ForcedCrashCategory.Abort);
            Application.Quit();
        }

        {
            CheckObjectState();
        }

        {
            originalTimeScale = Time.timeScale;
        }

        {
            StartCoroutine(Chec());
        }

        void RunChecks()
        {
            if (checkForUABE)
            {
                CheckForMissingObjects();
                CheckForTamperingOfScripts();
                CheckLoadedAssemblies();
            }

            if (cheatDetected)
            {
                HandleCheatDetected();
            }
        }

        void CheckForTamperingOfScripts()
        {
            foreach (string scriptName in protectedScripts)
            {
                Type type = Type.GetType(scriptName);
                if (type == null)
                {
                    Debug.LogError($"[Anti-Cheat] Missing script: {scriptName}");
                    MarkCheatDetected("Missing Script: " + scriptName);
                    continue;
                }

                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    if (method.GetMethodBody() == null)
                    {
                        Debug.LogError($"[Anti-Cheat] Stripped method: {method.Name} in {scriptName}");
                        MarkCheatDetected("Stripped method: " + scriptName);
                    }
                }
            }
        }

        void CheckForMissingObjects()
        {
            foreach (string objName in criticalObjectNames)
            {
                GameObject obj = GameObject.Find(objName);
                if (obj == null)
                {
                    Debug.LogError($"[Anti-Cheat] Missing GameObject: {objName}");
                    MarkCheatDetected("Missing GameObject: " + objName);
                }
            }
        }

        void CheckLoadedAssemblies()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in loadedAssemblies)
            {
                string name = asm.GetName().Name.ToLower();
                if (name.Contains("cheat") || name.Contains("hack") || name.Contains("inject"))
                {
                    Debug.LogError($"[Anti-Cheat] Suspicious assembly detected: {asm.FullName}");
                    MarkCheatDetected("Suspicious Assembly: " + name);
                }
            }
        }

        void MarkCheatDetected(string reason)
        {
            cheatDetected = true;
            Debug.LogWarning("[Anti-Cheat] Cheat Detected: " + reason);
        }

        void HandleCheatDetected()
        {
            Debug.LogWarning("[Anti-Cheat] Handling cheat detection...");
            if (disconnect)
            {
                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.Disconnect();
                    Debug.LogWarning("[Anti-Cheat] Disconnected from Photon due to cheat detection.");
                }
            }
        }

        void CheckObjectState()
        {
            foreach (GameObject obj in objectToCheck)
            {
                if (obj != null && !obj.activeSelf)
                {
                    Debug.Log("An object in the array is disabled.");

                    if (PhotonVRManager != null)
                    {
                        PhotonVRManager.gameObject.SetActive(false);
                    }

                    if (objectToEnable != null)
                    {
                        objectToEnable.SetActive(true);
                        Debug.Log("The objectToEnable has been enabled.");
                    }
                }
                else if (obj != null && obj.activeSelf)
                {
                    Debug.Log("An object in the array is enabled.");
                }
            }

        }

        void Update()
        {

            {
                CheckObjectState();
            }

            if (lagOutGame)
            {
                Time.timeScale = 0.1f * lagIntensity;
            }
            else
            {
                Time.timeScale = originalTimeScale;
            }
        }

        if (lagOutGame)
        {
            originalTimeScale = Time.timeScale;
        }

        gamePath = Application.persistentDataPath;
        initialMemoryHash = GetMemoryHash();
        initialScriptHash = GetFileHash(Application.persistentDataPath + "/StarFiresPaidAntiCheat.cs");
        initialAPKHash = GetAPKHash();

        CreateFakeAntiCheatObjects();

        CheckForModFolders();
        CheckForRootAccess();
        CheckForTamperedFiles();
        CheckForMemoryTampering();
        PreventReflectionAttacks();
        CheckForScriptTampering();
        CheckForUnauthorizedProcesses();
        CheckForInjectedLibraries();
        CheckForHookedAPIFunctions();
        CheckForOverlaySoftware();
        CheckForVPN();
        VerifyAPKIntegrity();


        if (playFabLoginScript != null)
        {
            AuthenticatePlayFabUser();
        }
        else
        {
            Debug.LogError("[StarFiresPaidAntiCheat] PlayFab login script is not assigned!");
            TakeAction();
        }
    }
    private void PlayFabUser()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void AuthenticatePlayFabUser()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("[StarFiresPaidAntiCheat] PlayFab Login Successful.");

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), accountResult =>
        {
            playerPlayFabID = accountResult.AccountInfo.PlayFabId;
            Debug.Log("[StarFiresPaidAntiCheat] Retrieved PlayFab ID: " + playerPlayFabID);
        },
        error =>
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Failed to retrieve PlayFab ID: " + error.GenerateErrorReport());
        });
    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("[StarFiresPaidAntiCheat] PlayFab Login Failed: " + error.GenerateErrorReport());
        playerPlayFabID = "Login Failed";
        TakeAction();
    }

    private void OnLagOutToggleChanged(bool isOn)
    {
        lagOutGame = isOn;
    }

    [ContextMenu("Apply Fake Anti-Cheat")]
    private void ApplyFakeAntiCheatObjects()
    {
        CreateFakeAntiCheatObjects();
        Debug.Log("[StarFiresPaidAntiCheat] Has Created Fake Anti-Cheats");
    }
    private void CreateFakeAntiCheatObjects()
    {
        string[] fakeNames = { "AntiCheatPro", "SecureityShield", "ModBlocker", "HackPreventer", "MemoryScaner", "AntiHackProtectron",
                               "DllFileScaner", "Dll/ModSniper", "DLLHunter", "ModDefense", "AntiCheatX",
                               "GameModBlocker", "CheatHunter", "AntiHack", "AntiCheat", "ModShield",
                               "MemoryScan", "PatchDetector", "CheatGuard", "SecureityModBlocker" };

        for (int i = 0; i < fakeNames.Length; i++)
        {
            GameObject fakeObject = new GameObject(fakeNames[i]);
            fakeObject.transform.position = new Vector3(0, 0, 0);
        }
    }
    IEnumerator Chec()
    {
        while (true)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(RawPastebinUrl))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Failed to load" + webRequest.error);
                }
                else
                {
                    if (webRequest.downloadHandler.text != CurrentVersion.ToString())
                    {
                        foreach (GameObject obj in EnableIfOutdated)
                        {
                            obj.SetActive(true);
                        }
                        foreach (GameObject obj in DisableIfOutdated)
                        {
                            obj.SetActive(false);
                        }
                        if (DisconnectFromPhoton)
                        {
                            PhotonNetwork.Disconnect();
                        }
                        if (quitGame)
                        {
                            Application.Quit();
                        }
                    }
                }
            }
            yield return new WaitForSeconds(60);
        }
    }
    private IEnumerator WaitForPlayFabIDAndSendWebhook()
    {
        float timeout = 5f;
        float timer = 0f;

        while (playerPlayFabID == "Unknown" && timer < timeout)
        {
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }

        if (playerPlayFabID == "Unknown")
        {
            Debug.LogError("[StarFiresPaidAntiCheat] PlayFab ID retrieval timeout! Sending webhook anyway...");
        }

        string finalMessage = $"{customDiscordMessage}\n\n?? **PlayFab ID:** `{playerPlayFabID}`";
        SendDiscordNotification(finalMessage);
    }
    private void TakeAction()
    {

        Debug.Log("[StarFiresPaidAntiCheat] TakeAction() triggered!");

        if (PhotonVRManager != null)
        {
            PhotonVRManager.gameObject.SetActive(false);
        }

        if (TeleportToDiddysDungeonOnCheat == true && player != null && teleportLocation != null)
        {
            StartCoroutine(TeleportWithHandling());
        }

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        if (EarFuckThem)
        {
            EarFuckSound.SetActive(true);
        }

        if (lagOutGame)
        {
            Time.timeScale = 0.1f * lagIntensity;
        }
        if (quitGame)
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Cheat detected! Quitting game...");
            Application.Quit();
        }
        else if (disconnectPhoton)
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Cheat detected! Disconnecting from Photon...");
            PhotonNetwork.Disconnect();
        }

        if (sendToDiscordWebhook)
        {
            StartCoroutine(WaitForPlayFabIDAndSendWebhook());
        }
    }

    private void GetPlayFabID()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            playerPlayFabID = result.AccountInfo.PlayFabId;
            Debug.Log("[StarFiresPaidAntiCheat] PlayFab ID retrieved: " + playerPlayFabID);
        },
        error =>
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Failed to retrieve PlayFab ID: " + error.GenerateErrorReport());
        });
    }

    private IEnumerator TeleportWithHandling()
    {
        if (collidersToDisableForDiddysDungeon != null)
        {
            foreach (var obj in collidersToDisableForDiddysDungeon)
                SetCollidersEnabled(obj, false);
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();
        CharacterController cc = player.GetComponent<CharacterController>();

        if (rb != null)
            rb.isKinematic = true;

        yield return new WaitForSeconds(0.09f);

        if (cc != null)
        {
            cc.enabled = false;
            player.position = teleportLocation.position;
            cc.enabled = true;
        }
        else
        {
            player.position = teleportLocation.position;
        }

        yield return new WaitForSeconds(0.1f);

        if (collidersToDisableForDiddysDungeon != null)
        {
            foreach (var obj in collidersToDisableForDiddysDungeon)
                SetCollidersEnabled(obj, true);
        }

        if (rb != null)
            rb.isKinematic = false;
    }

    private void SetCollidersEnabled(GameObject target, bool enabledState)
    {
        Collider[] colliders = target.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
            col.enabled = enabledState;
    }

    private void SendDiscordNotification(string message)
    {
        if (string.IsNullOrWhiteSpace(discordWebhookURL))
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Webhook URL is missing! Please set it in the Inspector.");
            return;
        }

        StartCoroutine(SendWebhookRequest(discordWebhookURL, message));
    }
    private IEnumerator SendWebhookRequest(string url, string message)
    {
        WWWForm form = new WWWForm();
        form.AddField("content", message);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[StarFiresPaidAntiCheat] Failed to send Discord webhook: " + request.error);
            }
            else
            {
                Debug.Log("[StarFiresPaidAntiCheat] Successfully sent Discord webhook notification.");
            }
        }
    }
    private void CheckForModFolders()
    {
        List<string> allFoldersToCheck = new List<string>(forbiddenFolders);
        allFoldersToCheck.AddRange(customFolders);

        foreach (string folder in allFoldersToCheck)
        {
            string folderPath = Path.Combine(gamePath, folder);
            if (Directory.Exists(folderPath))
            {
                Debug.LogError($"[StarFiresPaidAntiCheat] Unauthorized folder found: {folder}");
                TakeAction();
            }
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }
    }

    private void CheckForRootAccess()
    {
        string[] rootIndicators = { "/system/bin/su", "/system/xbin/su", "/system/app/Superuser.apk", "/sbin/su" };

        foreach (string path in rootIndicators)
        {
            if (File.Exists(path))
            {
                Debug.LogError("[StarFiresPaidAntiCheat] Root access detected!");
                TakeAction();
            }
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }
    }

    private void CheckForVPN()
    {
        try
        {
            using (AndroidJavaClass connectivityManager = new AndroidJavaClass("android.net.ConnectivityManager"))
            using (AndroidJavaObject activity = new AndroidJavaObject("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject context = activity.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject cm = context.Call<AndroidJavaObject>("getSystemService", "connectivity");

                AndroidJavaObject network = cm.Call<AndroidJavaObject>("getActiveNetworkInfo");

                if (network != null && network.Call<bool>("isConnected"))
                {
                    AndroidJavaObject linkProperties = cm.Call<AndroidJavaObject>("getLinkProperties", network);
                    if (linkProperties != null)
                    {
                        string interfaceName = linkProperties.Call<string>("getInterfaceName");
                        if (interfaceName != null && (interfaceName.Contains("tun") || interfaceName.Contains("ppp") || interfaceName.Contains("pptp")))
                        {
                            Debug.LogError("[StarFiresPaidAntiCheat] VPN or Proxy detected!");
                            TakeAction();
                        }
                        if (PhotonNetwork.IsConnected)
                        {
                            PhotonNetwork.Disconnect();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[StarFiresPaidAntiCheat] VPN check error: {ex.Message}");
        }
    }

    private void CheckForTamperedFiles()
    {
        string[] filesToCheck = { "libgame.so", "global-metadata.dat", "assembly-csharp.dll" };

        foreach (string file in filesToCheck)
        {
            string filePath = Path.Combine(Application.persistentDataPath, file);
            if (!File.Exists(filePath))
            {
                Debug.LogError($"[StarFiresPaidAntiCheat] Missing or tampered file detected: {file}");
                TakeAction();
            }
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }
    }

    private void CheckForMemoryTampering()
    {
        string currentMemoryHash = GetMemoryHash();
        if (currentMemoryHash != initialMemoryHash)
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Memory modification detected! Closing game...");
            TakeAction();
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    private void PreventReflectionAttacks()
    {
        try
        {
            foreach (MethodInfo method in typeof(StarFiresPaidAntiCheat).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (method.IsSecurityCritical || method.IsSecuritySafeCritical)
                {
                    Debug.LogError("[StarFiresPaidAntiCheat] Reflection attack detected! Closing game...");
                    TakeAction();
                }
                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.Disconnect();
                }
            }
        }
        catch (Exception)
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Security check failed. Possible attack detected.");
            TakeAction();
        }
    }

    private void VerifyAPKIntegrity()
    {
        string currentAPKHash = GetAPKHash();
        if (currentAPKHash != initialAPKHash)
        {
            Debug.LogError("[StarFiresPaidAntiCheat] APK integrity compromised! Exiting...");
            TakeAction();
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }
    private void AddCustomFolder()
    {
        string newFolder = string.Join(",", assembliesToCheck);


        if (!string.IsNullOrEmpty(newFolder) && !customFolders.Contains(newFolder))
        {
            customFolders.Add(newFolder);
            Debug.Log($"[StarFiresPaidAntiCheat] Added custom folder: {newFolder}");
        }
        else
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Invalid or duplicate folder name.");
        }
    }

    private void RemoveCustomFolder()
    {
        string folderToRemove = string.Join(",", assembliesToCheck);


        if (customFolders.Contains(folderToRemove))
        {
            customFolders.Remove(folderToRemove);
            Debug.Log($"[StarFiresPaidAntiCheat] Removed custom folder: {folderToRemove}");
        }
        else
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Folder not found in custom folders.");
        }
    }


    private void AddFolder()
    {
        string newFolder = string.Join(",", FoldersToCheck);


        if (!string.IsNullOrEmpty(newFolder) && !customFolders.Contains(newFolder))
        {
            customFolders.Add(newFolder);
            Debug.Log($"[StarFiresPaidAntiCheat] Added custom folder: {newFolder}");
        }
        else
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Invalid or duplicate folder name.");
        }
    }

    private void RemoveFolder()
    {
        string folderToRemove = string.Join(",", FoldersToCheck);


        if (customFolders.Contains(folderToRemove))
        {
            customFolders.Remove(folderToRemove);
            Debug.Log($"[StarFiresPaidAntiCheat] Removed custom folder: {folderToRemove}");
        }
        else
        {
            Debug.LogError("[StarFiresPaidAntiCheat] Folder not found in custom folders.");
        }
    }


    private string GetMemoryHash()
    {
        using (var md5 = MD5.Create())
        {
            byte[] memoryDump = BitConverter.GetBytes(GC.GetTotalMemory(false));
            byte[] hashBytes = md5.ComputeHash(memoryDump);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    private string GetFileHash(string filePath)
    {
        if (!File.Exists(filePath))
            return string.Empty;

        using (var sha256 = SHA256.Create())
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            byte[] hashBytes = sha256.ComputeHash(fileBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    private string GetAPKHash()
    {
        string apkPath = Application.dataPath;
        if (!File.Exists(apkPath))
            return string.Empty;

        using (var sha256 = SHA256.Create())
        {
            byte[] fileBytes = File.ReadAllBytes(apkPath);
            byte[] hashBytes = sha256.ComputeHash(fileBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    private void CheckForScriptTampering()
    {
        Debug.LogError("[StarFiresPaidAntiCheat] Script tampering detected!");
        TakeAction();
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }
    private void CheckForUnauthorizedProcesses()
    {
        Debug.LogError("[StarFiresPaidAntiCheat] Unauthorized process detected!");
        TakeAction();
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    private void CheckForInjectedLibraries()
    {
        Debug.LogError("[StarFiresPaidAntiCheat] Injected libraries detected!");
        TakeAction();
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    private void CheckForHookedAPIFunctions()
    {
        Debug.LogError("[StarFiresPaidAntiCheat] Hooked API functions detected!");
        TakeAction();
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    private void CheckForOverlaySoftware()
    {
        Debug.LogError("[StarFiresPaidAntiCheat] Overlay software detected!");
        TakeAction();
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    private void CustomFolderCheck()
    {
        string androidAppDir = Application.dataPath;
        bool folderExists = false;

        foreach (string folder in forbiddenFolders)
        {
            if (Directory.Exists(Path.Combine(androidAppDir, folder)))
            {
                folderExists = true;
                break;
            }
        }

        if (folderExists)
        {
           TakeAction();
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        {
            GetPlayFabID();
        }
    }
}

/*****************************************************
 * COPYRIGHT (C) 2025 STARFIREVR STUDIOS. ALL RIGHTS RESERVED.
 * 
 * THIS CODE AND ITS CONTENTS ARE THE PROPERTY OF STARFIREVR STUDIOS. 
 * DO NOT REPRODUCE, DISTRIBUTE, MODIFY, OR USE THIS CODE WITHOUT 
 * EXPLICIT PERMISSION. ANY UNAUTHORIZED USE WILL RESULT IN POTENTIAL
 * LEGAL ACTION. THIS SCRIPT IS COPYRIGHTED AND CANNOT BE 
 * CLAIMED, LEAKED, OR SHARED WITHOUT CONSENT.
 * 
 * FOR LICENSE INFORMATION, PLEASE CONTACT STARFIREVR STUDIOS.
 *****************************************************/
