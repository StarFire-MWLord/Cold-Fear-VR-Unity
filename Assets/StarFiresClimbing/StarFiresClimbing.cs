using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class StarFiresClimbing : MonoBehaviour
{
    [Header("Climbing Settings")]
    public LayerMask climbableSurfaceLayer;
    public GameObject climbingObjectPrefab;
    public EasyHand climbingHand;
    public GameObject Controller;
    private GameObject activeClimbingObject;
    private void OnCollisionEnter(Collision collision)
    {
        if ((climbableSurfaceLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (EasyInputs.GetGripButtonDown(climbingHand))
            {
                SpawnClimbingObject(collision);
            }
        }
    }
    private void SpawnClimbingObject(Collision collision)
    {
        activeClimbingObject = Instantiate(climbingObjectPrefab, transform.position, Quaternion.identity);
        activeClimbingObject.transform.SetParent(collision.transform);
        transform.SetParent(collision.transform);
    }
}