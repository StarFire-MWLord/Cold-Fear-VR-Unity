using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StarfiresPhotonManger : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class ServerEntry
    {
        public string punAppId;
        public string voiceAppId;
    }
    [Header("Photon Settings")]
    [Space]
    public string defaultPunAppId = "";
    [Space]
    public string defaultVoiceAppId = "";
    [Space]
    public byte maxPlayersPerRoom = 10;
    [Space]
    [Header("Startup Behavior")]
    public bool AutoConnectToPhoton = true;
    public bool AutoJoinRoomOnAwake = true;
    [HideInInspector] public List<ServerEntry> publicServers = new List<ServerEntry>();
    [HideInInspector] public List<ServerEntry> privateServers = new List<ServerEntry>();
    private int currentServerIndex = -1;
    private bool joiningPrivateRoom = false;
    private string targetRoomCode = "";
    private const int MaxPrivateRoomsPerServer = 2;

    void Awake()
    {
        if (AutoConnectToPhoton)
        {
            ConnectToServer(FindDefaultServerIndex(), isPrivate: false);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon.");

        if (joiningPrivateRoom && !string.IsNullOrEmpty(targetRoomCode))
        {
            PhotonNetwork.JoinRoom(targetRoomCode);
        }
        else if (AutoJoinRoomOnAwake)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public void JoinRoomByCode(string code)
    {
        bool isPrivate = !Regex.IsMatch(code, @"^\d+$");

        if (isPrivate)
        {
            joiningPrivateRoom = true;
            targetRoomCode = code;
            ConnectToPrivateServerWithCapacity();
        }
        else
        {
            joiningPrivateRoom = false;
            targetRoomCode = code;
            ConnectToServer(FindDefaultServerIndex(), isPrivate: false);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"JoinRoom failed: {message}");

        if (joiningPrivateRoom && returnCode == ErrorCode.GameDoesNotExist)
        {
            TryCreatePrivateRoom(targetRoomCode);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room: " + message);
        if (joiningPrivateRoom)
        {
            currentServerIndex++;
            ConnectToPrivateServerWithCapacity();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"JoinRandomRoom failed on server index {currentServerIndex}: {message}");
        TryNextPublicServer();
    }

    int FindDefaultServerIndex()
    {
        return publicServers.Count > 0 ? 0 : -1;
    }

    void ConnectToServer(int index, bool isPrivate)
    {
        var serverList = isPrivate ? privateServers : publicServers;

        if (index < 0 || index >= serverList.Count)
        {
            Debug.LogError("Server index out of range.");
            return;
        }

        currentServerIndex = index;
        ServerEntry server = serverList[index];

        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = server.punAppId;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = server.voiceAppId;

        PhotonNetwork.ConnectUsingSettings();
    }

    void ConnectToPrivateServerWithCapacity()
    {
        for (int i = 0; i < privateServers.Count; i++)
        {
            int privateRoomCount = CountPrivateRoomsOnServer();
            if (privateRoomCount < MaxPrivateRoomsPerServer)
            {
                ConnectToServer(i, isPrivate: true);
                return;
            }
        }

        Debug.LogError("No private backup server with capacity available.");
    }

    void TryNextPublicServer()
    {
        for (int i = currentServerIndex + 1; i < publicServers.Count; i++)
        {
            Debug.Log($"Trying next public server: index {i}");
            ConnectToServer(i, isPrivate: false);
            return;
        }

        Debug.LogError("All public servers are full or unavailable.");
    }

    void TryCreatePrivateRoom(string code)
    {
        int privateRoomCount = CountPrivateRoomsOnServer();
        if (privateRoomCount >= MaxPrivateRoomsPerServer)
        {
            Debug.LogWarning("Private server full, trying next.");
            ConnectToPrivateServerWithCapacity();
            return;
        }

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = maxPlayersPerRoom,
            IsVisible = false,
            IsOpen = true
        };
        PhotonNetwork.CreateRoom(code, options);
    }

    int CountPrivateRoomsOnServer()
    {
        return 0;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(StarfiresPhotonManger))]
    public class StarfiresPhotonMangerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawSection("This Script Was Made By StarFireVR", () =>
            {
                EditorGUILayout.HelpBox(
                 "StarFireStudios © 2025 to 2035\n" +
                 "All rights reserved.\n\n" +
                 "This script is the intellectual property of StarFireStudios.\n\n" +
                 "You may NOT claim this script or any part of it as your own work.\n\n" +
                 "If you purchased this script through our official Discord server or store:\n" +
                 "- You are granted a personal-use license only.\n" +
                 "- You may NOT share, redistribute, or resell this script in any form.\n\n" +
                 "If you obtained this script as a PUBLIC release from our official Discord server:\n" +
                 "- You are allowed to share and redistribute it freely.\n" +
                 "- You still may NOT claim it as your own creation or remove this copyright notice.\n\n" +
                 "Unauthorized copying, redistribution, or misrepresentation of this script is a violation\n" +
                 "of copyright and may result in legal action or blacklisting from StarFireStudios projects.\n\n" +
                 "For questions or licensing inquiries, contact us through our official Discord or store page.",
                 MessageType.Info
                );
            });

            DrawDefaultInspector();

            SerializedProperty publicServersProp = serializedObject.FindProperty("publicServers");
            SerializedProperty privateServersProp = serializedObject.FindProperty("privateServers");

            EditorGUILayout.Space(10);
            DrawServerList(publicServersProp, "Public Servers");
            EditorGUILayout.Space(10);
            DrawServerList(privateServersProp, "Private Servers");

            serializedObject.ApplyModifiedProperties();
        }

        void DrawSection(string header, System.Action content)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            content.Invoke();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        void DrawServerList(SerializedProperty listProp, string label)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            for (int i = 0; i < listProp.arraySize; i++)
            {
                SerializedProperty entry = listProp.GetArrayElementAtIndex(i);
                SerializedProperty punAppId = entry.FindPropertyRelative("punAppId");
                SerializedProperty voiceAppId = entry.FindPropertyRelative("voiceAppId");

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"{label} {i + 1}", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(punAppId, new GUIContent("PUN App ID"));
                EditorGUILayout.PropertyField(voiceAppId, new GUIContent("Voice App ID"));
                if (GUILayout.Button("Remove"))
                {
                    listProp.DeleteArrayElementAtIndex(i);
                    break;
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button($"Add {label}"))
            {
                listProp.arraySize++;
            }
        }
    }
#endif
}
/*
 * StarFireStudios © 2025 to 2035
 * All rights reserved.
 *
 * This script is the intellectual property of StarFireStudios.
 *
 * You may NOT claim this script or any part of it as your own work.
 *
 * If you purchased this script through our official Discord server or store:
 * - You are granted a personal-use license only.
 * - You may NOT share, redistribute, or resell this script in any form.
 *
 * If you obtained this script as a PUBLIC release from our official Discord server:
 * - You are allowed to share and redistribute it freely.
 * - You still may NOT claim it as your own creation or remove this copyright notice.
 *
 * Unauthorized copying, redistribution, or misrepresentation of this script is a violation
 * of copyright and may result in legal action or blacklisting from StarFireStudios projects.
 *
 * For questions or licensing inquiries, contact us through our official Discord or store page.
 */