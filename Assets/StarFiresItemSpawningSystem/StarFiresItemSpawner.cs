using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class StarFiresItemSpawner : MonoBehaviour
{
    [Header("This Script Was Made By StarFireVR")]
    [Space]
    public StarFiresItemManager spawnManager;
    public int selectedItemIndex;

    void OnValidate()
    {
        if (spawnManager == null)
        {
            spawnManager = FindObjectOfType<StarFiresItemManager>();
        }

        if (spawnManager != null && spawnManager.itemPrefabs != null)
        {
            if (selectedItemIndex >= spawnManager.itemPrefabs.Count)
                selectedItemIndex = 0;
        }
    }

    void Start()
    {
        if (!Application.isPlaying)
            return;

        if (spawnManager != null &&
            spawnManager.itemPrefabs != null &&
            selectedItemIndex >= 0 &&
            selectedItemIndex < spawnManager.itemPrefabs.Count)
        {
            GameObject prefabToSpawn = spawnManager.itemPrefabs[selectedItemIndex];
            if (prefabToSpawn != null)
            {
                Instantiate(prefabToSpawn, transform.position, transform.rotation);
            }
        }
    }
}