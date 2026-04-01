using UnityEngine;
using System.IO;

public class Melonloaderchecker : MonoBehaviour
{
    void Start()
    {
        CheckForMelonloaderFolder();
    }

    void CheckForMelonloaderFolder()
    {
        string modsFolderPath = Path.Combine(Application.persistentDataPath, "melonloader");

        if (Directory.Exists(modsFolderPath))
        {
            Debug.LogError("MelonLoader folder detected. Exiting application.");
            Application.Quit();
        }
    }
}