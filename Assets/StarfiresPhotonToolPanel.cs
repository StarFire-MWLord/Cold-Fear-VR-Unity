using UnityEngine;
using UnityEditor;
using Photon.Pun;
using Photon.Realtime;

public class StarfiresPhotonToolPanel : EditorWindow
{
    private string privateRoomCode = "";

    [MenuItem("StarFiresTools/StarFiresPhotonToolPanel")]
    public static void ShowWindow()
    {
        GetWindow<StarfiresPhotonToolPanel>("Starfire's Photon Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Photon Connection Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Connect to Photon"))
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        {
            if (GUILayout.Button("Disconnect from Photon"))
            {
                PhotonNetwork.Disconnect();
            }
        }

        GUILayout.Label("Room Connection Settings", EditorStyles.boldLabel);

        

        if (GUILayout.Button("Join Random Room"))
        {
            PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.ConnectUsingSettings();
        }

        if (GUILayout.Button("Leave Random Room"))
        {
            PhotonNetwork.Disconnect();
        }

        privateRoomCode = EditorGUILayout.TextField("Private Room Code:", privateRoomCode);

        if (GUILayout.Button("Join Private Room"))
        {
          PhotonNetwork.LeaveRoom();
        }
        else
        {
            if (!string.IsNullOrEmpty(privateRoomCode))
                PhotonNetwork.JoinRoom(privateRoomCode);
        }

        if (GUILayout.Button("Leave Private Room"))
        {
            PhotonNetwork.Disconnect();
        }


        GUI.enabled = PhotonNetwork.InRoom;

        GUI.enabled = true;
    }
}
//This Script Was Made By StarFireVR!!!