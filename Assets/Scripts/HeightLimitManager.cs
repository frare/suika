using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeightLimitManager : MonoBehaviour
{
    public bool IsLimitReached => isLimitReached;

    [SerializeField] private float maxLimitTimer = 10f;
    [SerializeField] private BoxCollider2D detectionBox;
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private SpriteRenderer limitRenderer;

    private bool isLimitReached;
    private float currentLimitTimer = 0f;

    private ContactFilter2D contactFilter;
    private readonly List<Collider2D> contactResults = new();

    private void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(detectionLayers);
        contactFilter.useTriggers = false;
    }

    private void FixedUpdate()
    {
        isLimitReached = ScanForItems();

        if (isLimitReached)
        {
            currentLimitTimer += Time.fixedDeltaTime;
            if (currentLimitTimer >= maxLimitTimer)
            {
                GameManager.Instance.GameOver();
            }
        }
        else currentLimitTimer = 0f;

        UpdateLimitGraphic(isLimitReached);
    }

    private bool ScanForItems()
    {
        detectionBox.Overlap(contactFilter, contactResults);
        foreach (var hit in contactResults)
        {
            if (hit.attachedRigidbody.TryGetComponent(out Item item))
            {
                if (item.IsReady)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void UpdateLimitGraphic(bool isLimitReached)
    {
        if (isLimitReached)
        {
            limitRenderer.color = new Color(1f, 0f, 0f, 0.2f);
        }
        else
        {
            limitRenderer.color = new Color(1f, 1f, 1f, 0.2f);
        }
    }
}