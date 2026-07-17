using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BatteryVisibilityController : MonoBehaviour
{
    public Light2D playerLight;  // must be Light2D

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerLight == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerLight = player.GetComponentInChildren<Light2D>();  // **IMPORTANT**: Light2D here!
            }

            if (playerLight == null)
            {
                Debug.LogWarning("Player Light2D component not found automatically. Please assign it manually.");
            }
        }
    }

    void Update()
    {
        if (playerLight == null || spriteRenderer == null)
            return;

        float distance = Vector2.Distance(transform.position, playerLight.transform.position);

        spriteRenderer.enabled = distance <= playerLight.pointLightOuterRadius;
    }
}
