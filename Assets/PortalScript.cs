using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Portals
{
    public class PortalScript : MonoBehaviourPunCallbacks
    {
        public float DurationUntilSpawn = 60f;
        public GameObject Portal;
        public bool SpawnPortal;

        [SerializeField] private float timeRemaining;
        private bool hasSpawned = false;

        void Start()
        {
            ResetTimerInternal();
        }

        void Update()
        {
            if (!HasNetworkSession())
            {
                if (Portal != null && Portal.activeSelf)
                    Portal.SetActive(false);

                hasSpawned = false;
                timeRemaining = DurationUntilSpawn;
                return;
            }
            
            if (hasSpawned) return;

            timeRemaining -= UnityEngine.Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                if (PhotonNetwork.InRoom)
                    SpawnPortalNetworked();
                else
                    SpawnPortalLocal();
            }
            if (SpawnPortal)
            {
                if (PhotonNetwork.InRoom)
                    SpawnPortalNetworked();
                else
                    SpawnPortalLocal();
            }
        }

        private bool HasNetworkSession()
        {
            return PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode && (PhotonNetwork.InRoom || PhotonNetwork.InLobby);
        }

        private void SpawnPortalLocal()
        {
            if (Portal == null)
            {
                Debug.LogWarning("PortalScript: Portal is not assigned.");
                return;
            }

            Portal.SetActive(true);
            hasSpawned = true;
        }

        private void SpawnPortalNetworked()
        {
            if (Portal == null)
            {
                Debug.LogWarning("PortalScript: Portal is not assigned.");
                return;
            }
            photonView.RPC(nameof(RPC_SetPortalActive), RpcTarget.AllBuffered, true);
            hasSpawned = true;
        }

        private void ResetTimerInternal()
        {
            timeRemaining = DurationUntilSpawn;
        }

        public void RestartTimer()
        {
            ResetTimerInternal();
            hasSpawned = false;

            if (PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode && PhotonNetwork.InRoom)
            {
                photonView.RPC(nameof(RPC_SetPortalActive), RpcTarget.AllBuffered, false);
            }
            else
            {
                if (Portal != null)
                    Portal.SetActive(false);
            }
        }

        public float GetTimeRemaining()
        {
            return Mathf.Max(0f, timeRemaining);
        }

        public bool IsSpawned()
        {
            return hasSpawned;
        }

        [PunRPC]
        private void RPC_SetPortalActive(bool active)
        {
            if (Portal != null)
                Portal.SetActive(active);

            hasSpawned = active;
        }
    }
}