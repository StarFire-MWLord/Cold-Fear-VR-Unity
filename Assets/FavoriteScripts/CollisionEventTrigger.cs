using UnityEngine;
using UnityEngine.Events;

public class CollisionEventTrigger : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onEnter;
    public UnityEvent onExit;

    [Header("Options")]
    public bool requireTrigger = true; // only fire if one collider is a trigger
    public string[] requiredTags;      // array of tags that can trigger the event

    private bool Valid(Collider other)
    {
        // If tags are set, collider must match at least one
        if (requiredTags != null && requiredTags.Length > 0)
        {
            bool tagMatch = false;
            foreach (string tag in requiredTags)
            {
                if (other.CompareTag(tag))
                {
                    tagMatch = true;
                    break;
                }
            }
            if (!tagMatch) return false;
        }

        // If requireTrigger is true, at least one collider must be a trigger
        if (requireTrigger)
        {
            if (!other.isTrigger && !GetComponent<Collider>().isTrigger)
                return false;
        }

        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!requireTrigger && Valid(collision.collider))
            onEnter?.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!requireTrigger && Valid(collision.collider))
            onExit?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Valid(other))
            onEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (Valid(other))
            onExit?.Invoke();
    }
}