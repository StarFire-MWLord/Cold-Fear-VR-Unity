using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using Photon;
using Photon.Pun;
using Photon.Realtime;
//using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class OculusAuthenticator : MonoBehaviour
{
    [Header("THE SCRIPT WAS MADE BY STARFIRE")]
    [Space]

    private string oculusId;
    public LoadBalancingClient loadBalancingClient;
    public GameObject objectToDisable1;
    public GameObject objectToDisable2;
    public GameObject objectToEnable1;

    private void Start()
    {
        Core.AsyncInitialize().OnComplete(OnInitializationCallback);
    }

    private void OnInitializationCallback(Message<PlatformInitialize> msg)
    {
        if (msg.IsError)
        {
            Debug.LogErrorFormat("Oculus: Error during initialization. Error Message: {0}",
                msg.GetError().Message);
            DisableOrEnableObjects();
        }
        else
        {
            Entitlements.IsUserEntitledToApplication().OnComplete(OnIsEntitledCallback);
        }
    }

    private void OnIsEntitledCallback(Message msg)
    {
        if (msg.IsError)
        {
            Debug.LogErrorFormat("Oculus: Error verifying the user is entitled to the application. Error Message: {0}",
                msg.GetError().Message);
            DisableOrEnableObjects();
        }
        else
        {
            GetLoggedInUser();
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
            Debug.LogErrorFormat("Oculus: Error getting logged in user. Error Message: {0}",
                msg.GetError().Message);
            DisableOrEnableObjects();
        }
        else
        {
            oculusId = msg.Data.ID.ToString(); // do not use msg.Data.OculusID;
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
            Debug.LogErrorFormat("Oculus: Error getting user proof. Error Message: {0}",
                msg.GetError().Message);
            DisableOrEnableObjects();
        }
        else
        {
            string oculusNonce = msg.Data.Value;
            loadBalancingClient.AuthValues = new AuthenticationValues();
            loadBalancingClient.AuthValues.UserId = oculusId;
            loadBalancingClient.AuthValues.AuthType = CustomAuthenticationType.Oculus;
            loadBalancingClient.AuthValues.AddAuthParameter("userid", oculusId);
            loadBalancingClient.AuthValues.AddAuthParameter("nonce", oculusNonce);
        }
    }

    private void DisableOrEnableObjects()
    {
        if (objectToDisable1 != null) objectToDisable1.SetActive(false);
        if (objectToDisable2 != null) objectToDisable2.SetActive(false);
        if (objectToEnable1 != null) objectToEnable1.SetActive(true);
    }
}
