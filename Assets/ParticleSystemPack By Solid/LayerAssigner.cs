using UnityEngine;

public class LayerAssigner : MonoBehaviour
{
    [Tooltip("Sorting Layer Index to assign to the Particle System(s)")]
    public int sortingLayerIndex = 0; // Set this to your desired sorting layer index

    void Start()
    {
        // Get all sorting layers
        var layers = SortingLayer.layers;
        if (sortingLayerIndex < 0 || sortingLayerIndex >= layers.Length)
        {
            Debug.LogWarning("Invalid sorting layer index.");
            return;
        }

        int sortingLayerID = layers[sortingLayerIndex].id;

        // Assign sorting layer to this GameObject if it has a ParticleSystem
        var ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var renderer = ps.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sortingLayerID = sortingLayerID;
        }

        // Assign sorting layer to all child Particle Systems
        foreach (var childPs in GetComponentsInChildren<ParticleSystem>())
        {
            var renderer = childPs.GetComponent<Renderer>();
            if (renderer != null)
                renderer.sortingLayerID = sortingLayerID;
        }
    }
}
