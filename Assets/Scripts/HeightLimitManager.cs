using System.Collections.Generic;
using UnityEngine;

public class HeightLimitManager : MonoBehaviour
{
    public bool IsLimitReached => isLimitReached;

    [SerializeField] private float maxLimitTimer = 10f;
    [SerializeField] private MeshRenderer limitRenderer;

    private bool isLimitReached;
    private float limitTimer;
    private Material limitMaterial;
    private readonly List<Item> itemsInsideLimit = new();

    private void Awake()
    {
        limitMaterial = limitRenderer.material;
    }

    private void Update()
    {
        if (isLimitReached)
        {
            limitTimer -= Time.deltaTime;
            if (limitTimer <= 0 )
            {
                Debug.Log($"{name}:game over, items overlapping the limit for too long");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out Item item))
        {
            if (item.FinishedMoving && !itemsInsideLimit.Contains(item))
            {
                itemsInsideLimit.Add(item);
                UpdateLimit();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out Item item))
        {
            if (item.FinishedMoving && itemsInsideLimit.Contains(item))
            {
                itemsInsideLimit.Remove(item);
                UpdateLimit();
            }
        }
    }

    private void UpdateLimit()
    {
        isLimitReached = itemsInsideLimit.Count > 0;

        if (isLimitReached)
        {
            limitTimer = maxLimitTimer;
            limitMaterial.color = new Color(1f, 0f, 0f, 0.2f);
        }
        else
        {
            limitTimer = 0f;
            limitMaterial.color = new Color(1f, 1f, 1f, 0.2f);
        }
    }
}