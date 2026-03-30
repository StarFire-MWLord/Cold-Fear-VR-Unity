using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class DisplayPlayFabID : MonoBehaviour
{
    [Header("PlayFab Display")]
    [SerializeField] private Playfablogin playfabLoginScript;
    [SerializeField] private TextMeshPro idDisplayText;

    private void Start()
    {
        if (playfabLoginScript == null || idDisplayText == null)
        {
            Debug.LogError("DisplayPlayFabID: Missing references to Playfablogin or TextMeshPro.");
        }
    }

    private void Update()
    {
        UpdatePlayFabIDDisplay();
    }

    private void UpdatePlayFabIDDisplay()
    {
        if (playfabLoginScript != null && !string.IsNullOrEmpty(playfabLoginScript.MyPlayFabID))
        {
            idDisplayText.text = $"Your ID is: {playfabLoginScript.MyPlayFabID}";
        }
    }
}


//Script Created by StarFireVR