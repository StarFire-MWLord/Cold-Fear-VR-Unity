using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFiresAntiTamper : MonoBehaviour
{
    [Header("PUT THIS AS YOUR OBJECT TO CHECK IF DISABLED")]
    public GameObject objectToCheck;

    [Header("PUT THIS AS YOUR PHOTON VR MANAGER")]
    [Space]
    public GameObject objectToDisable;

    [Header("PUT THIS AS YOUR FAILED TO AUTHENTICATE YOU OBJECT")]
    [Space]
    public GameObject objectToEnable;

    
    public bool toggleState;

    void Start()
    {
        CheckObjectState();
    }

    void Update()
    {
        CheckObjectState();
    }

    void CheckObjectState()
    {
        if (objectToCheck != null && !objectToCheck.activeSelf)
        {
            Debug.Log("The object is  disabled.");

            if (objectToDisable != null)
            {
                objectToDisable.SetActive(false);
                Debug.Log("The object has been disabled.");
            }

            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
                Debug.Log("The object has been enabled.");
            }
        }
        else if (objectToCheck != null && objectToCheck.activeSelf)
        {
            Debug.Log("The object is enabled.");
        }
    }

    public void ToggleObjectState()
    {
        if (objectToCheck != null)
        {
            objectToCheck.SetActive(toggleState);
            Debug.Log("The objectToCheck has been " + (toggleState ? "enabled" : "disabled"));
        }
        else
        {
            Debug.LogError("No object assigned to check.");
        }
    }
}