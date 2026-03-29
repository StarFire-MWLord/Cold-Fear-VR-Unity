using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using Photon.Pun;
using Unity.XR.Oculus.Input;

public class StarFiresOculusAuthManager : MonoBehaviour
{
    [Header("This Script Was Made By StarFireVR")]
    [Space]
    [Header("Use This To Disable Oculus Authentication When Testing In The Unity Editor")]
    [Space]
    public bool DisableOculusAuthInEditor;
    [Space]
    [Header("Quit Game Or Disconnect From Photon On Auth Failure")]
    [Space]
    public bool DisconnectPhotonOnAuthFailure;
    [Space]
    public bool QuitGameOnAuthFailure;
    [Space]
    [Header("Objects To Enable Or Disable On - Auth Success")]
    [Space]
    public List<GameObject> onAuthSuccessEnableObjects = new List<GameObject>();
    [Space]
    public List<GameObject> onAuthSuccessDisableObjects = new List<GameObject>();
    [Space]
    [Header("Objects To Enable Or Disable On - Auth Failure")]
    [Space]
    public List<GameObject> onAuthFailEnableObjects = new List<GameObject>();
    [Space]
    public List<GameObject> onAuthFailDisableObjects = new List<GameObject>();

    private string oculusId;

    void Start()
    {
        InvokeRepeating(nameof(StartAuthSequence), 0f, 30f);
    }

    void OnDestroy()
    {
        CancelInvoke(nameof(StartAuthSequence));
    }

    private void StartAuthSequence()
    {
        #if UNITY_EDITOR
        if (DisableOculusAuthInEditor)
        {
            Debug.Log("Oculus Authentication Skipped In Editor Mode.");
            return;
        }
        #endif

        if (!Core.IsInitialized())
        {
            try
            {
                Core.Initialize();
                Entitlements.IsUserEntitledToApplication().OnComplete(OnAuthCallback);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Oculus Initialization Error: " + e.Message);
                HandleAuthFailed();
            }
        }
        else
        {
            Entitlements.IsUserEntitledToApplication().OnComplete(OnAuthCallback);
        }
    }

    private void OnAuthCallback(Message msg)
    {
        if (msg.IsError)
        {
            Debug.LogError("Oculus Authentication Failed: " + msg.GetError().Message);
            HandleAuthFailed();
        }
        else
        {
            Debug.Log("Oculus Authentication Successful.");
            GetLoggedInUser();
        }
    }

    private void HandleAuthSuccess()
    {
        SetObjectsActive(onAuthSuccessEnableObjects, true);
        SetObjectsActive(onAuthSuccessDisableObjects, false);
    }

    private void HandleAuthFailed()
    {
        SetObjectsActive(onAuthFailEnableObjects, true);
        SetObjectsActive(onAuthFailDisableObjects, false);

        if (DisconnectPhotonOnAuthFailure)
        {
            PhotonNetwork.Disconnect();
            Debug.LogError("Disconnecting Photon Auth Failure.");
        }

        if (QuitGameOnAuthFailure)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            UnityEngine.Application.Quit();
            #else
            UnityEngine.Application.Quit();
            #endif
            Debug.LogError("Quitting Game Auth Failure.");
        }
    }

    private void SetObjectsActive(List<GameObject> objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(isActive);
        }
    }

    private void GetLoggedInUser()
    {
        Users.GetLoggedInUser().OnComplete(OnLoggedInUserCallback);
    }

    private void OnLoggedInUserCallback(Message<User> msg)
    {
        if (msg.IsError)
        {
            Debug.LogErrorFormat("Oculus: Error getting logged in user. Error Message: {0}", msg.GetError().Message);
            HandleAuthFailed();
        }
        else
        {
            oculusId = msg.Data.ID.ToString();
            GetUserProof();
        }
    }

    private void GetUserProof()
    {
        Users.GetUserProof().OnComplete(OnUserProofCallback);
    }

    private void OnUserProofCallback(Message<UserProof> msg)
    {
        if (msg.IsError)
        {
            Debug.LogErrorFormat("Oculus: Error getting user proof. Error Message: {0}", msg.GetError().Message);
            HandleAuthFailed();
        }
        else
        {
            Debug.Log("Oculus: Successfully retrieved user proof.");
            HandleAuthSuccess();
        }
    }
}