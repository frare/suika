using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundEffectController : MonoBehaviour
{
    private Material defaultMaterial;
    private Material runtimeMaterial;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        defaultMaterial = spriteRenderer.sharedMaterial;
        runtimeMaterial = new Material(defaultMaterial);

        spriteRenderer.sharedMaterial = runtimeMaterial;
    }

    private void OnDestroy()
    {
        if (spriteRenderer != null && spriteRenderer.sharedMaterial == runtimeMaterial)
        {
            spriteRenderer.sharedMaterial = defaultMaterial;
        }

        if (runtimeMaterial != null) Destroy(runtimeMaterial);
    }
}