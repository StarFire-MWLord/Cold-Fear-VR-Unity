using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SceneManagement : MonoBehaviour
{
    [SerializeField]
    public string SceneToGo;
    public string HandTag;
    public float CooldownToScene;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(HandTag))
        {
            Invoke("LoadScene", CooldownToScene);
            Debug.Log("loading scene...");
        }
        else
        {
            Debug.LogError("failed to load scene. please try setting it up again.");
        }
    }

    void LoadScene()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToGo);
        Debug.Log("loaded scene: " + SceneToGo);
    }
}
