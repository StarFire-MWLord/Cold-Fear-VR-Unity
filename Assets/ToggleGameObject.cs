using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using easyInputs;

public class ToggleGameObject : MonoBehaviour
{
    public GameObject[] objectsToToggle;
    public float cooldown = 2.0f;
    public AudioSource FlashlightSound;
    public AudioClip FlashlightClip;
    public Key Keys = Key.F;

    private float lastActivatedTime = -Mathf.Infinity;

    private void Update()
    {
        if (Time.time - lastActivatedTime > cooldown)
        {
            if (EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand))
            {
                foreach (GameObject objects in objectsToToggle)
                {
                    objects.SetActive(!objects.activeSelf);
                }
                lastActivatedTime = Time.time;
                FlashlightSound.clip = FlashlightClip;
                FlashlightSound.Play();
            }
        }
        if (Time.time - lastActivatedTime > cooldown)
        {
            if (Keyboard.current[Keys].isPressed)
            {
                foreach (GameObject objects in objectsToToggle)
                {
                    objects.SetActive(!objects.activeSelf);
                }
                lastActivatedTime = Time.time;
                FlashlightSound.clip = FlashlightClip;
                FlashlightSound.Play();
            }
        }
    }
}
