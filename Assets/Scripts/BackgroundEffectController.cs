using System;
using UnityEngine;

public class BackgroundEffectController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;
    [SerializeField] private Vector2 tilingFactor;
    [SerializeField] private Color backgroundColor;
    [SerializeField] private Color patternColor;

    private void Awake()
    {
        ApplyMaterialValues();
    }

    private void OnValidate()
    {
        ApplyMaterialValues();
    }

    private void ApplyMaterialValues()
    {
        try
        {
            spriteRenderer.material.SetFloat("_SpeedX", speedX);
            spriteRenderer.material.SetFloat("_SpeedY", speedY);
            spriteRenderer.material.SetVector("_TilingFactor", tilingFactor);
            spriteRenderer.material.SetColor("_BackgroundColor", backgroundColor);
            spriteRenderer.material.SetColor("_PatternColor", patternColor);
        }
        catch (Exception e)
        {
            Debug.LogError($"{name}:{e}");
        }
    }
}