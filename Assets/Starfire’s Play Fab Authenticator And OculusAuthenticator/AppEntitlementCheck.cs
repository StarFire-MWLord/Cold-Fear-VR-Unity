using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections.Generic;
using Photon.Pun;

namespace Oculus.Platform.EntitlementCheck
{
    public class AppEntitlementCheck : MonoBehaviour
    {
        [Header("DO NOT FILL OUT")]
        public string oculusDisplayName;
        public string oculusUsername;
        public string oculusPlayerID;
        public static AppEntitlementCheck instance { get; private set; }

        void Awake()
        {
            try
            {
                Core.AsyncInitialize();
                Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);

                Users.GetLoggedInUser().OnComplete(m =>
                {
                    if (!m.IsError && m.Type == Message.MessageType.User_GetLoggedInUser)
                    {
                        User user = m.GetUser();
                        oculusPlayerID = user.ID.ToString();
                        oculusUsername = user.OculusID;


                        oculusDisplayName = user.DisplayName;
                        Users.GetUserProof().OnComplete(r =>
                        {
                            //grrrrrr
                        });
                    }

                });
            }
            catch (UnityException e)
            {
                Debug.LogError("Platform failed to initialize due to exception.");
                Debug.LogException(e);
            }
        }

        void EntitlementCallback(Message msg)
        {
            if (msg.IsError)
            {
                Debug.LogError("You are NOT entitled to use this app.");
            }
            else
            {
                Debug.Log("You are entitled to use this app.");
            }
        }
    }
}