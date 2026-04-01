using UnityEngine;
using PlayFab;

public class PlayerPlayFabId : MonoBehaviour
{
    public string playFabId;

    void Start()
    {
        playFabId = PlayFabSettings.staticPlayer.PlayFabId;
    }
}