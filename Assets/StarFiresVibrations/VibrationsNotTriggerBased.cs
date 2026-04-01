using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class VibrationsNotTriggerBased : MonoBehaviour
{
    [Header("This Script Was Made By StarFireVR")]
    [Space]
    [Header("Vibration Settings")]
    [SerializeField] private float vibrationStrength = 0.2f;

    [Header("Hand Layers")]
    [SerializeField] private LayerMask LeftHandLayer;
    [SerializeField] private LayerMask RightHandLayer;

    [Header("Vibration Settings")]
    [SerializeField] private float collisionVelocityThreshold = 0.1f; // Minimum velocity to trigger vibration
    [SerializeField] private float vibrationCooldown = 0.2f; // Cooldown per hand

    private float lastLeftVibrationTime = 0f;
    private float lastRightVibrationTime = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.collider.gameObject.layer;
        float velocity = collision.relativeVelocity.magnitude;

        // LEFT HAND
        if (((1 << layer) & LeftHandLayer) != 0 && velocity > collisionVelocityThreshold)
        {
            if (Time.time - lastLeftVibrationTime > vibrationCooldown)
            {
                lastLeftVibrationTime = Time.time;
                Debug.Log("Left hand hit ground.");
                StartCoroutine(EasyInputs.Vibration(EasyHand.LeftHand, vibrationStrength, 0.2f)); // fixed 0.2s
            }
        }
        // RIGHT HAND
        else if (((1 << layer) & RightHandLayer) != 0 && velocity > collisionVelocityThreshold)
        {
            if (Time.time - lastRightVibrationTime > vibrationCooldown)
            {
                lastRightVibrationTime = Time.time;
                Debug.Log("Right hand hit ground.");
                StartCoroutine(EasyInputs.Vibration(EasyHand.RightHand, vibrationStrength, 0.2f)); // fixed 0.2s
            }
        }
    }
}