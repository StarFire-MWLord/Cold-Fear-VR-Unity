using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisablnoAdio : MonoBehaviour
{
    public GameObject[] ObjectsToDisable;
    public GameObject[] ObjectsToEnable;
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
        }
    }
}
