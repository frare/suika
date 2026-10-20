using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    public bool IsReady => isReady;
    public Vector2 Position => rb.position;
    public Sprite SpriteGraphic => spriteRenderer.sprite;
    public float SpawnOrder => spawnOrder;

    public Action OnReady;
    public Action OnDestroying;

    [HideInInspector] public bool isDespawning;

    [SerializeField] protected int score = 0;
    [SerializeField] protected Item nextItem;
    
    [Header("References")]
    [SerializeField] protected Transform spriteContainer;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected static long nextSpawnOrder;
    
    protected bool wasShot = false;
    protected bool isReady = true;
    protected long spawnOrder;

    protected Rigidbody2D rb;
    protected Collider2D mergeCollider;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mergeCollider = GetComponent<Collider2D>();

        spawnOrder = nextSpawnOrder++;

        // bounce animation
        spriteContainer.localScale = Vector3.one;
        spriteContainer.DOScale(1.2f, 0.1f)
            .SetLoops(2, LoopType.Yoyo);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (wasShot && !isReady)
        {
            isReady = true;
            OnReady?.Invoke();
        }

        if (collision.collider.attachedRigidbody != null && collision.collider.attachedRigidbody.TryGetComponent(out Item otherItem))
        {
            TryMerge(otherItem);
        }
    }

    protected virtual void OnDestroy()
    {
        // cleanup for DOTween
        spriteContainer.DOKill();
    }

    public virtual void Freeze()
    {
        isReady = false;
        mergeCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public virtual void Release()
    {
        wasShot = true;
        transform.parent = null;
        mergeCollider.enabled = true;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public virtual void Despawn()
    {
        isDespawning = true;

        OnReady?.Invoke();
        OnDestroying?.Invoke();

        Destroy(this.gameObject);
    }

    protected virtual void TryMerge(Item otherItem)
    {
        // dont try to merge if final tier
        if (nextItem == null) return;

        if (this.nextItem == otherItem.nextItem
        && this.isDespawning == false && otherItem.isDespawning == false)
        {
            // OLDER handles the merging and persists
            // the other gets destroyed
            if (this.spawnOrder < otherItem.spawnOrder) MergeToNextLevel(otherItem);
        }
    }

    protected virtual void MergeToNextLevel(Item otherItem)
    {
        Debug.Log($"{transform.GetInstanceID()}: entered MergeToNextLevel {otherItem.transform.GetInstanceID()}");

        var targetPosition = (rb.position + otherItem.Position) / 2f;

        Instantiate(nextItem.gameObject, targetPosition, Quaternion.Euler(new Vector3(0f, 0f, rb.rotation)));

        ParticlesManager.Instance.PlayMergeParticles(targetPosition);

        GameManager.Instance.AddScore(this.score + otherItem.score);

        otherItem.Despawn();
        this.Despawn();
    }
}