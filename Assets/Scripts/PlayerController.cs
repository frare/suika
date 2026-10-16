using UnityEngine;
using UnityEngine.InputSystem;
using frare.Vector3Extensions;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform aimPoint;
    [SerializeField] private BoxCollider2D aimZone;
    [SerializeField] private float moveSpeed;

    private Vector3 moveInput;
    private Item currentItem;

    private void Start()
    {
        SetupNewItem();
    }

    private void Update()
    {
        if (!Vector3Extensions.Approximately(moveInput, Vector3.zero))
        {
            MoveAimClampedToZone(aimPoint.position + (moveSpeed * Time.deltaTime * moveInput));
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
    }
#endif

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (currentItem != null)
        {
            currentItem.Release();
        }
    }

    public void OnPointer(InputValue value)
    {
        MoveAimClampedToZone(Camera.main.ScreenToWorldPoint(value.Get<Vector2>()));
    }

    private void SetupNewItem()
    {
        currentItem = ItemSpawner.Instance.SpawnItem(aimPoint);
        currentItem.Freeze();
        currentItem.OnFinishedMoving += OnItemFinishedMoving;
    }

    private void OnItemFinishedMoving()
    {
        if (currentItem != null) currentItem.OnFinishedMoving -= OnItemFinishedMoving;
        currentItem = null;
        SetupNewItem();
    }

    private void MoveAimClampedToZone(Vector3 targetPosition)
    {
        aimPoint.position = targetPosition;

        // clamp to the aim zone
        var resultX = Mathf.Clamp(aimPoint.position.x, aimZone.bounds.min.x, aimZone.bounds.max.x);
        var resultY = Mathf.Clamp(aimPoint.position.y, aimZone.bounds.min.y, aimZone.bounds.max.y);

        // Z is always 0 since this is a 2D game
        aimPoint.position = new Vector3(resultX, resultY, 0f);
    }
}