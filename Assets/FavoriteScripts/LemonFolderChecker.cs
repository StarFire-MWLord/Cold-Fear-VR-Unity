using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LemonFolderChecker : MonoBehaviour
{
    void Start()
    {
        CheckForMelonloaderFolder();
    }

    void CheckForMelonloaderFolder()
    {
        string modsFolderPath = Path.Combine(Application.persistentDataPath, "lemonloader");

        if (Directory.Exists(modsFolderPath))
        {
            Debug.LogError("MelonLoader folder detected. Exiting application.");
            Application.Quit();
        }
    }
}
