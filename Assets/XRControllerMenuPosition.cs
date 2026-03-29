using UnityEngine;
using UnityEngine.XR;

public class XRControllerMenuPosition : MonoBehaviour
{
    [Header("Controller References")]
    [Tooltip("Reference to the Left Controller's Transform.")]
    public Transform leftController;

    [Header("Target Transforms")]
    [Tooltip("Target transform for the Right Controller when the menu is active and tracked.")]
    public Transform rightMenuTransform;
    [Tooltip("Target transform for the Right Controller when it is disconnected or disabled.")]
    public Transform rightMoveToPositionTransform;
    [Tooltip("Target transform for the Left Controller when it is disconnected or disabled.")]
    public Transform leftMoveToPositionTransform;

    [Header("Testing Options")]
    [Tooltip("Simulate menu activation in the Editor.")]
    public bool enableTest = false;
    [Tooltip("Simulate that the Left Controller is disabled or not tracked.")]
    public bool leftDisableTest = false;
    [Tooltip("Simulate that the Right Controller is disabled or not tracked.")]
    public bool rightDisableTest = false;

    private bool menuActive = false;

    void Update()
    {
        if (enableTest)
        {
            if (!menuActive)
            {
                menuActive = true;
                Debug.Log("Menu activated via test flag.");
            }
        }
        else
        {
            InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if (rightDevice.isValid)
            {
                bool menuButtonPressed = false;
                rightDevice.TryGetFeatureValue(CommonUsages.menuButton, out menuButtonPressed);
                
                if (menuButtonPressed && !menuActive)
                {
                    menuActive = true;
                    Debug.Log("Menu activated via XR input.");
                }
                else if (!menuButtonPressed && menuActive)
                {
                    menuActive = false;
                    Debug.Log("Menu deactivated via XR input.");
                }
            }
        }
    }

    void LateUpdate()
    {
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        
        bool leftTracked = false;
        bool rightTracked = false;
        
        if (leftDevice.isValid)
            leftDevice.TryGetFeatureValue(CommonUsages.isTracked, out leftTracked);
        
        if (rightDevice.isValid)
            rightDevice.TryGetFeatureValue(CommonUsages.isTracked, out rightTracked);

        if (menuActive)
        {
            if (rightTracked && !rightDisableTest)
            {
                if (rightMenuTransform != null)
                    transform.position = rightMenuTransform.position;
            }
            else
            {
                if (rightMoveToPositionTransform != null)
                    transform.position = rightMoveToPositionTransform.position;
            }
        }
        else  
        {
            if (!rightTracked || rightDisableTest)
            {
                if (rightMoveToPositionTransform != null)
                    transform.position = rightMoveToPositionTransform.position;
            }
        }

        if (leftController != null)
        {
            if (menuActive)
            {
                if (!leftTracked || leftDisableTest)
                {
                    if (leftMoveToPositionTransform != null)
                        leftController.position = leftMoveToPositionTransform.position;
                }
            }
            else
            {
                if (!leftTracked || leftDisableTest)
                {
                    if (leftMoveToPositionTransform != null)
                        leftController.position = leftMoveToPositionTransform.position;
                }
            }
        }
    }
}