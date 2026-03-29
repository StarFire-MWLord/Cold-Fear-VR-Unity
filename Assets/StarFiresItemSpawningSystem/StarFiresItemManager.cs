using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFiresItemManager : MonoBehaviour
{
    [Header("This Script Was Made By StarFireVR")]
    [Space]
    public List<GameObject> itemPrefabs;

    public string[] GetItemNames()
    {
        var names = new string[itemPrefabs.Count];
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            names[i] = itemPrefabs[i] != null ? itemPrefabs[i].name : "None";
        }
        return names;
    }
}