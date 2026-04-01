using GorillaLocomotion;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class GorillaRigRespawn : MonoBehaviour
{
    public GameObject GorillaPlayer;
    public GameObject teleportTarget;
    public List<Collider> collidersToDisable;
    public float timeDisabled;

    public void OnTriggerEnter(Collider other)
    {
        foreach (Collider collider in collidersToDisable)
        {
            collider.enabled = false;
        }
        GorillaPlayer.transform.position = teleportTarget.transform.position;
        StartCoroutine(EnableColliders());
    }

    private IEnumerator EnableColliders()
    {
        yield return new WaitForSeconds(timeDisabled);

        foreach (Collider collider in collidersToDisable)
        {
            collider.enabled = true;
        }
    }
}
