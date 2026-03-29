#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System;

public class StarFiresFreeSceneObjectEncryptionSystem : EditorWindow
{
    private string encryptionKey = "";
    private Vector2 scrollPos;
    private Dictionary<GameObject, bool> skipObjects = new Dictionary<GameObject, bool>();

    private static Action pendingAction;
    private static float startTime;
    private static float duration;
    private static string progressTitle;
    private static string progressMessage;

    [MenuItem("StarFiresTools/StarFiresFreeSceneObjectEncryptionSystem")]
    public static void ShowWindow()
    {
        GetWindow<StarFiresFreeSceneObjectEncryptionSystem>("Scene Encryption Object System");
    }

    private void OnEnable()
    {
        RefreshSceneObjects();
    }

    private void OnHierarchyChange()
    {
        RefreshSceneObjects();
    }

    private void RefreshSceneObjects()
    {
        skipObjects.Clear();
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (string.IsNullOrEmpty(obj.scene.name)) continue; // Skip prefabs and non-scene objects
            if (!skipObjects.ContainsKey(obj))
                skipObjects[obj] = false;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("StarFire's Free Scene Object Encryption System", EditorStyles.boldLabel);

        encryptionKey = "tFeDKLwCn6Bo3l3m";

        if (encryptionKey.Length != 16)
        {
            EditorGUILayout.HelpBox("Encryption key must be exactly 16 characters.", MessageType.Warning);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Encrypt All"))
        {
            if (ValidateKey())
                ShowProgressBarBeforeAction("Encrypting", "Encrypting...", EncryptAll);
        }

        if (GUILayout.Button("Decrypt All"))
        {
            if (ValidateKey())
                ShowProgressBarBeforeAction("Decrypting", "Decrypting...", DecryptAll);
        }
    }

    private bool ValidateKey()
    {
        if (encryptionKey.Length != 16)
        {
            EditorUtility.DisplayDialog("Invalid Key", "The encryption key must be exactly 16 characters long.", "OK");
            return false;
        }
        return true;
    }

    private void ShowProgressBarBeforeAction(string title, string message, Action action)
    {
        startTime = (float)EditorApplication.timeSinceStartup;
        duration = UnityEngine.Random.Range(5f, 8f);
        progressTitle = title;
        progressMessage = message;
        pendingAction = action;

        EditorApplication.update += SimulateLoadingBar;
    }

    private void SimulateLoadingBar()
    {
        float elapsed = (float)(EditorApplication.timeSinceStartup - startTime);
        float progress = Mathf.Clamp01(elapsed / duration);
        EditorUtility.DisplayProgressBar(progressTitle, progressMessage, progress);

        if (elapsed >= duration)
        {
            EditorApplication.update -= SimulateLoadingBar;
            EditorUtility.ClearProgressBar();

            pendingAction?.Invoke();
            pendingAction = null;
        }
    }

    private void EncryptAll()
    {
        var objs = skipObjects.Keys.Where(obj => obj != null && !skipObjects[obj]).ToList();
        for (int i = 0; i < objs.Count; i++)
        {
            GameObject obj = objs[i];
            obj.name = EncryptString(obj.name);
            EditorUtility.SetDirty(obj);
        }

        Debug.Log("Encryption Complete!");
    }

    private void DecryptAll()
    {
        var objs = skipObjects.Keys.Where(obj => obj != null && !skipObjects[obj]).ToList();
        for (int i = 0; i < objs.Count; i++)
        {
            GameObject obj = objs[i];
            obj.name = DecryptString(obj.name);
            EditorUtility.SetDirty(obj);
        }

        Debug.Log("Decryption Complete!");
    }

    private string EncryptString(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16];

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return System.Convert.ToBase64String(encryptedBytes);
        }
    }

    private string DecryptString(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16];

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] cipherBytes = System.Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
#endif
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