using UnityEngine;
using System.IO;

public class ModsFolderChecker : MonoBehaviour
{
    void Start()
    {
        CheckForModsFolder();
    }

    void CheckForModsFolder()
    {
        string modsFolderPath = Path.Combine(Application.persistentDataPath, "Mods");

        if (Directory.Exists(modsFolderPath))
        {
            Debug.LogError("Mods folder detected. Exiting application.");
            Application.Quit();
        }
    }
}