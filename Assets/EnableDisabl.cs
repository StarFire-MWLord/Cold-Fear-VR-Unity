using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnableDisabl : MonoBehaviour
{
    public GameObject[] ObjectsToDisable;
    public GameObject[] ObjectsToEnable;
    public AudioSource ButtonSound;
    public AudioClip[] ButtonClip;
    public float PitchMin = 0.5f;
    public float PitchMax = 1.5f;
    public string HandTag = "Untagged";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == HandTag)
        {
            if (ObjectsToDisable != null && ObjectsToEnable != null && ObjectsToDisable.Length > 0 && ObjectsToEnable.Length > 0)
            {
                foreach (GameObject obj in ObjectsToDisable)
                {
                    obj.SetActive(false);
                }

                foreach (GameObject obj in ObjectsToEnable)
                {
                    obj.SetActive(true);
                }
            }

            if (ButtonSound != null && ButtonClip != null && ButtonClip.Length > 0)
            {
                ButtonSound.clip = ButtonClip[Random.Range(0, ButtonClip.Length)];
                ButtonSound.pitch = Random.Range(PitchMin, PitchMax);
                ButtonSound.Play();
            }
        }
    }
}
