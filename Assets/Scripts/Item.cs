using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    public bool FinishedMoving => finishedMoving;
    public Sprite SpriteGraphic => spriteRenderer.sprite;

    public Action OnFinishedMoving;
    public Action OnDestroying;

    [HideInInspector] public float spawnTime;
    [HideInInspector] public bool isDespawning;

    [SerializeField] protected int score = 0;
    [SerializeField] protected Item nextItem;
    
    [Header("References")]
    [SerializeField] protected Transform spriteContainer;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected float currentScale = 1f;
    protected bool wasShot = false;
    protected bool finishedMoving = false;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        spawnTime = Time.time;

        // bounce animation
        spriteContainer.localScale = Vector3.one;
        spriteContainer.DOScale(1.2f, 0.1f)
            .SetLoops(2, LoopType.Yoyo);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (wasShot && !finishedMoving)
        {
            finishedMoving = true;
            OnFinishedMoving?.Invoke();
        }

        TryMerge(collision);
    }

    protected virtual void OnDestroy()
    {
        spriteContainer.DOKill();
    }

    public virtual void Freeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public virtual void Release()
    {
        wasShot = true;
        transform.parent = null;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public virtual void Despawn()
    {
        isDespawning = true;

        OnFinishedMoving?.Invoke();
        OnDestroying?.Invoke();

        Destroy(this.gameObject);
    }

    protected virtual void TryMerge(Collision2D collision)
    {
        // dont try to merge if final tier
        if (nextItem == null) return;

        if (collision.collider.attachedRigidbody != null && collision.collider.attachedRigidbody.TryGetComponent(out Item otherItem))
        {
            if (this.nextItem == otherItem.nextItem
            && this.isDespawning == false && otherItem.isDespawning == false)
            {
                // OLDER handles the merging and persists
                // the other gets destroyed
                if (this.spawnTime < otherItem.spawnTime) MergeToNextLevel(otherItem);
            }
        }
    }

    protected virtual void MergeToNextLevel(Item other)
    {
        GameManager.Instance.AddScore(this.score + other.score);
        Instantiate(nextItem.gameObject, rb.position, Quaternion.Euler(new Vector3(0f, 0f, rb.rotation)));
        ParticlesManager.Instance.PlayMergeParticles(rb.position);

        other.Despawn();
        this.Despawn();
    }
}