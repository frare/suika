using UnityEngine;
using UnityEngine.InputSystem;
using frare.Vector3Extensions;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform aimPoint;
    [SerializeField] private BoxCollider2D aimZone;
    [SerializeField] private float moveSpeed;

    private float AimRadiusOffset
    {
        get
        {
            if (currentItem == null) return 0f;
            else return currentItem.Radius + 0.02f; // add a small offset so the items don't bump into the lateral walls
        }
    }

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
        currentItem.OnReady += OnItemFinishedMoving;
    }

    private void OnItemFinishedMoving()
    {
        if (currentItem != null) currentItem.OnReady -= OnItemFinishedMoving;
        currentItem = null;
        SetupNewItem();
    }

    private void MoveAimClampedToZone(Vector3 targetPosition)
    {
        aimPoint.position = targetPosition;

        // clamp to the aim zone
        var resultX = Mathf.Clamp(aimPoint.position.x, aimZone.bounds.min.x + AimRadiusOffset, aimZone.bounds.max.x - AimRadiusOffset);
        var resultY = aimZone.transform.position.y;

        // Z is always 0 since this is a 2D game
        aimPoint.position = new Vector3(resultX, resultY, 0f);
    }
}