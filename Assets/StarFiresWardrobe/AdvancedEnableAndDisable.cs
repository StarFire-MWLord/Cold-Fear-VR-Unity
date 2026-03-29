using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AdvancedEnableAndDisable : MonoBehaviour
{
    [Header("Created by StarFireVR")]
    [Space]
    [Header("Tag For Your Hand")]
    [SerializeField] private string handTag = "HandTag";
    [Space]
    [Header("Objects to Manage")]
    [SerializeField] private List<GameObject> objectsToEnable;
    [SerializeField] private List<GameObject> objectsToDisable;
    [Space]
    [Header("Settings")]
    [SerializeField] private float actionDelay = 0.5f;
    [SerializeField] private bool isNetworked = false;

    private PhotonView photonView;

    private void Start()
    {
        if (isNetworked)
        {
            photonView = GetComponent<PhotonView>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsHandTrigger(other))
        {
            Debug.Log($"Triggered with {other.gameObject.name}");
            StartCoroutine(HandleObjectStates());
        }
    }

    private bool IsHandTrigger(Collider other)
    {
        return other.gameObject.CompareTag(handTag);
    }

    private IEnumerator HandleObjectStates()
    {
        yield return HandleObjectsStateChange(objectsToEnable, true);
        yield return HandleObjectsStateChange(objectsToDisable, false);
    }

    private IEnumerator HandleObjectsStateChange(List<GameObject> objectList, bool enable)
    {
        foreach (var obj in objectList)
        {
            yield return new WaitForSeconds(actionDelay);
            if (obj != null)
            {
                Debug.Log($"{(enable ? "Enabling" : "Disabling")} object: {obj.name}");
                if (isNetworked)
                {
                    HandleNetworkedObjectState(obj, enable);
                }
                else
                {
                    SetObjectState(obj, enable);
                }
            }
            else
            {
                Debug.LogWarning($"{(enable ? "Enable" : "Disable")} list contains a null object.");
            }
        }
    }

    private void HandleNetworkedObjectState(GameObject obj, bool enable)
    {
        if (photonView.IsMine)
        {
            SetObjectState(obj, enable);
            photonView.RPC(enable ? "RPC_EnableObject" : "RPC_DisableObject", RpcTarget.OthersBuffered, obj.GetComponent<PhotonView>().ViewID);
        }
    }

    private void SetObjectState(GameObject obj, bool enable)
    {
        obj.SetActive(enable);
    }

    [PunRPC]
    private void RPC_EnableObject(int viewID)
    {
        UpdateObjectState(viewID, true);
    }

    [PunRPC]
    private void RPC_DisableObject(int viewID)
    {
        UpdateObjectState(viewID, false);
    }

    private void UpdateObjectState(int viewID, bool enable)
    {
        PhotonView targetPhotonView = PhotonView.Find(viewID);
        if (targetPhotonView != null)
        {
            Debug.Log($"{(enable ? "Enabling" : "Disabling")} object via RPC: {targetPhotonView.gameObject.name}");
            targetPhotonView.gameObject.SetActive(enable);
        }
        else
        {
            Debug.LogError($"Could not find object with ViewID: {viewID}.");
        }
    }
}