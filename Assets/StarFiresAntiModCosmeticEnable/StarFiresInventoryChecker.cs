using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;

public class StarFiresInventoryChecker : MonoBehaviour
{
    [Header("Script Created by StarFireVR")]
    [Space]
    [Header("Item Check Settings")]
    public GameObject objectToCheck;
    public string itemId;
    public string catalogName;
    private bool autoCheckOnStart = true;

    private void Start()
    {
        if (autoCheckOnStart)
        {
            StartCoroutine(DelayedInventoryCheck());
        }
    }

    private IEnumerator DelayedInventoryCheck()
    {
        yield return new WaitForSeconds(1f);
        CheckInventoryIfObjectEnabled();
    }

    public void CheckInventoryIfObjectEnabled()
    {
        if (objectToCheck == null)
        {
            Debug.LogWarning("No object assigned to check!");
            return;
        }

        if (!objectToCheck.activeInHierarchy)
        {
            Debug.Log("⏭️ Object is not enabled. Skipping inventory check.");
            return;
        }

        if (Playfablogin.instance == null || Playfablogin.instance.userInventory == null)
        {
            Debug.LogError("❌ Playfablogin or inventory not initialized yet.");
            return;
        }

        string catalog = string.IsNullOrEmpty(catalogName) ? Playfablogin.instance.CatalogName : catalogName;
        bool hasItem = false;

        foreach (ItemInstance item in Playfablogin.instance.userInventory)
        {
            if (item.CatalogVersion == catalog && item.ItemId == itemId)
            {
                hasItem = true;
                break;
            }
        }

        if (hasItem)
        {
            Debug.Log($"✅ Player owns the item: {itemId}");
        }
        else
        {
            Debug.LogWarning($"❌ Player does NOT own the item: {itemId}. Kicking...");
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            UnityEngine.Application.Quit();
            #else
            Application.Quit();
            #endif
        }
    }
}