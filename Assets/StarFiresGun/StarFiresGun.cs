using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class StarFiresGun : MonoBehaviour
{
    [Header("This Script Ias Made By StarFireVR")]
    public AudioClip shootSound;
    public AudioSource audioSource;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.1f;
    private float lastShootTime = 0f;
    public EasyHand triggerhand;
    private bool isFullyAutomatic = true;
    public KeyCode testShootKey = KeyCode.Space;

    void Update()
    {
        if (isFullyAutomatic)
        {
            if (EasyInputs.GetTriggerButtonDown(triggerhand) && Time.time - lastShootTime >= shootCooldown)
            {
                Shoot();
            }
        }
        else
        {
            if (EasyInputs.GetTriggerButtonDown(triggerhand) && Time.time - lastShootTime >= shootCooldown)
            {
                Shoot();
            }
        }

        if (Input.GetKeyDown(testShootKey) && Time.time - lastShootTime >= shootCooldown)
        {
            Shoot();
        }

        if (EasyInputs.GetTriggerButtonDown(triggerhand))
        {
            ToggleFireMode();
        }

    }

    void Shoot()
    {
        audioSource.PlayOneShot(shootSound);

        if (bulletPrefab != null && shootPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = shootPoint.forward * 20f;
            }
        }

        lastShootTime = Time.time;
    }

    void ToggleFireMode()
    {
        isFullyAutomatic = !isFullyAutomatic;
        Debug.Log("Fire Mode: " + (isFullyAutomatic ? "Fully Automatic" : "Semi-Automatic"));
    }
}
