using DG.Tweening;
using frare.Vector3Extensions;
using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool FinishedMoving => finishedMoving;

    public Action onFinishedMoving;
    public Action onDestroying;

    [HideInInspector] public float spawnTime;
    [HideInInspector] public bool isDespawning;

    [SerializeField] protected int itemId = -1;
    [SerializeField] protected int score = 0;
    [SerializeField] protected Item nextItem;
    [SerializeField] protected float drag = 1.5f;
    [SerializeField] protected float rotationDrag = 0.5f;
    [SerializeField] protected float shootVelocity = 10f;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Collider mergeTrigger;
    [SerializeField] protected Transform mesh;

    protected float currentScale = 1f;
    protected bool wasShot = false;
    protected bool finishedMoving = false;

    protected virtual void Awake()
    {
        rb.linearDamping = drag;
        rb.angularDamping = rotationDrag;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        spawnTime = Time.time;

        // bounce animation
        mesh.localScale = Vector3.one;
        mesh.DOScale(1.2f, 0.1f)
            .SetLoops(2, LoopType.Yoyo);

        if (itemId == -1) Debug.LogError($"{name}:item id is -1! set an item id for this prefab");
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (wasShot && !finishedMoving)
        {
            finishedMoving = true;
            onFinishedMoving?.Invoke();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // dont try to merge if final tier
        if (nextItem == null) return;

        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out Item otherItem))
        {
            if (this.itemId == otherItem.itemId
                && (this.isDespawning == false && otherItem.isDespawning == false))
            {
                // OLDER handles the merging and persists
                // other will get destroyed
                if (this.spawnTime < otherItem.spawnTime)
                    MergeToNextLevel(otherItem);
            }
        }
    }

    public virtual void Freeze()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public virtual void Release()
    {
        wasShot = true;
        transform.parent = null;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    public virtual void Despawn()
    {
        isDespawning = true;

        onFinishedMoving?.Invoke();
        onDestroying?.Invoke();

        Destroy(this.gameObject);
    }

    public virtual void MergeToNextLevel(Item other)
    {
        GameManager.Instance.AddScore(this.score + other.score);
        Instantiate(nextItem.gameObject, rb.position, rb.rotation);

        other.Despawn();
        this.Despawn();
    }

    private void OnDrawGizmos()
    {
        SphereCollider trigger = (mergeTrigger as SphereCollider);
        if (trigger == null) return;

        Gizmos.color = Color.white;
        Vector3 triggerCenter = trigger.transform.TransformPoint(trigger.center);
        Gizmos.DrawWireSphere(triggerCenter, trigger.radius * currentScale);
    }
}