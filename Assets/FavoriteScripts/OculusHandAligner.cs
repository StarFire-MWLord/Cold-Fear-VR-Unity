using System.Collections;
using UnityEngine;
using easyInputs; // Use your EasyInputs namespace

public class EasyInputHandCalibrator : MonoBehaviour
{
    [Header("Hand to Calibrate")]
    public Transform hand;                  // The hand transform to calibrate
    public EasyHand easyHand = EasyHand.RightHand;  // Which hand to check for button

    [Header("Calibration Settings")]
    public bool allowRotationX = true;
    public bool allowRotationY = true;
    public bool allowRotationZ = true;

    private bool isCalibrating = false;

    void Update()
    {
        // Corrected: use EasyInputs class directly
        if (EasyInputs.GetPrimaryButtonDown(easyHand))
        {
            isCalibrating = !isCalibrating;
            Debug.Log(isCalibrating ? "Calibration mode ENABLED" : "Calibration mode LOCKED");
        }

        if (isCalibrating && hand != null)
        {
            RotateHand();
        }
    }

    private void RotateHand()
    {
        // Use mouse movement for testing; can later switch to VR thumbstick
        float rotationSpeed = 90f * Time.deltaTime;

        float rotX = allowRotationX ? Input.GetAxis("Mouse Y") * rotationSpeed : 0f;
        float rotY = allowRotationY ? Input.GetAxis("Mouse X") * rotationSpeed : 0f;
        float rotZ = allowRotationZ ? 0f : 0f; // Optional: add keys for Z-axis

        hand.Rotate(rotX, rotY, rotZ, Space.Self);
    }
}