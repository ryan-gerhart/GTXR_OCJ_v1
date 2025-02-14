using Fusion;
using UnityEngine;

public class HideOwnCostumeFusion : NetworkBehaviour
{
    private void Start()
    {
        // Check if this player has state authority
        if (Object.HasStateAuthority)
        {
            // Hide the costume visually by disabling the renderers
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }
    }
}   