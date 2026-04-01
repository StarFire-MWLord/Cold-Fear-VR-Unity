using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class VibrationsNotTriggerBased : MonoBehaviour
{
    [Header("This Script Was Made By StarFireVR")]
    [Space]
    [Header("Vibration Settings")]
    [SerializeField] private float vibrationStrength = 1.0f;
    [SerializeField] public EasyHand VibrationHand = EasyHand.LeftHand;

    private void Start()
    {
        if (VibrationHand != EasyHand.LeftHand && VibrationHand != EasyHand.RightHand)
        {
            Debug.Log("VibrationHand not set, defaulting to LeftHand.");
            VibrationHand = EasyHand.LeftHand;
        }
        else
        {
            Debug.Log("VibrationHand is set to: " + VibrationHand);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("HandTag"))
        {
            Debug.Log("Trigger Entered with HandTag.");
            TriggerVibration();
        }
        else
        {
            Debug.Log("Trigger entered, but not with HandTag.");
        }
    }

    private void TriggerVibration()
    {
        if (VibrationHand == EasyHand.LeftHand)
        {
            Debug.Log("Triggering vibration for LeftHand.");
            StartCoroutine(EasyInputs.Vibration(EasyHand.LeftHand, vibrationStrength, 0.5f));
        }
        else if (VibrationHand == EasyHand.RightHand)
        {
            Debug.Log("Triggering vibration for RightHand.");
            StartCoroutine(EasyInputs.Vibration(EasyHand.RightHand, vibrationStrength, 0.5f));
        }
        else
        {
            Debug.LogWarning("VibrationHand is not assigned properly.");
        }
    }
}
