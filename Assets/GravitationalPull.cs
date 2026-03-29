using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GravitationalPull : MonoBehaviour
{
    public float GravitationalRange = 5f;
    public float Strength = 10f;
    public string TargetTag = "Player";
    public LayerMask AffectedLayers = ~0;
    public bool DebugMode = false;
    public ForceMode ForceModeForRigidbodies = ForceMode.Acceleration;

    const float k_MinDistance = 0.1f;

    void FixedUpdate()
    {
        FindPlayers();
    }

    void FindPlayers()
    {
        if (GravitationalRange <= 0f || Mathf.Approximately(Strength, 0f))
        {
            if (DebugMode) Debug.LogWarning("GravitationalPull: GravitationalRange <= 0 or Strength == 0, nothing to do.");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, GravitationalRange, AffectedLayers);

        if (DebugMode) Debug.Log($"GravitationalPull: OverlapSphere found {hits.Length} collider(s).");

        for (int i = 0; i < hits.Length; i++)
        {
            Collider col = hits[i];
            if (col == null) continue;

            Rigidbody rb = col.attachedRigidbody ?? col.GetComponentInParent<Rigidbody>();
            if (rb == null)
            {
                if (DebugMode) Debug.Log($"Collider '{col.name}' has no Rigidbody attached/above it.");
                continue;
            }

            bool tagOk = string.IsNullOrEmpty(TargetTag)
                         || rb.gameObject.CompareTag(TargetTag)
                         || col.gameObject.CompareTag(TargetTag)
                         || col.transform.root.CompareTag(TargetTag);

            if (!tagOk)
            {
                if (DebugMode) Debug.Log($"Skipping '{rb.gameObject.name}' - tags don't match ('{rb.gameObject.tag}','{col.gameObject.tag}')");
                continue;
            }

            if (rb.isKinematic)
            {
                if (DebugMode) Debug.Log($"Skipping '{rb.gameObject.name}' because Rigidbody is kinematic.");
                continue;
            }

            Vector3 rbPos = rb.worldCenterOfMass;
            Vector3 toCenter = transform.position - rbPos;
            float distance = toCenter.magnitude;
            if (distance < k_MinDistance) distance = k_MinDistance;

            float t = Mathf.Clamp01(1f - (distance / GravitationalRange));
            Vector3 acceleration = toCenter.normalized * Strength * t;

            rb.AddForce(acceleration, ForceModeForRigidbodies);

            if (DebugMode)
            {
                Debug.DrawLine(rbPos, transform.position, Color.cyan, 0.1f);
                Debug.Log($"Applied force to '{rb.gameObject.name}': accel={acceleration:F3} distance={distance:F2} falloff={t:F2}");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.15f);
        Gizmos.DrawSphere(transform.position, GravitationalRange);
        Gizmos.color = new Color(0.2f, 0.6f, 1f, 1f);
        Gizmos.DrawWireSphere(transform.position, GravitationalRange);
    }
}