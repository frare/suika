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
    private float limitTimer;

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
        if (ScanForItems())
        {
            limitTimer -= Time.fixedDeltaTime;
            if (limitTimer <= 0 )
            {
                GameManager.Instance.GameOver();
            }
        }
        else limitTimer = maxLimitTimer;

        UpdateLimitGraphic(isLimitReached);
    }

    private bool ScanForItems()
    {
        isLimitReached = false;

        detectionBox.Overlap(contactFilter, contactResults);
        foreach (var hit in contactResults)
        {
            if (hit.attachedRigidbody.TryGetComponent(out Item item))
            {
                if (item.IsReady)
                {
                    isLimitReached = true;
                    break;
                }
            }
        }

        return isLimitReached;
    }

    private void UpdateLimitGraphic(bool isLimitReached)
    {
        if (isLimitReached)
        {
            limitTimer = maxLimitTimer;
            limitRenderer.color = new Color(1f, 0f, 0f, 0.2f);
        }
        else
        {
            limitTimer = 0f;
            limitRenderer.color = new Color(1f, 1f, 1f, 0.2f);
        }
    }
}