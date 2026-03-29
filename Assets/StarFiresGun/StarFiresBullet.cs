using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFiresBullet : MonoBehaviour
{
    [Header("This Script Ias Made By StarFireVR")]
    public float speed = 30f;
    public float lifetime = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
    }
}