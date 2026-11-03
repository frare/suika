using UnityEngine;
using UnityEngine.InputSystem;
using frare.Vector3Extensions;
using frare.CoroutineHelper;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float shootCooldown;
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

    private Coroutine shootTimerCoroutine;
    private Vector3 moveInput;
    private Item currentItem;

#region UNITY CALLBACKS
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
#endregion

#region INPUT SYSTEM CALLBACKS
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (currentItem != null)
        {
            currentItem.Release();
            CoroutineHelper.StartCoroutineOverride(this, ref shootTimerCoroutine, ShotCooldown());
        }
    }

    public void OnPointer(InputValue value)
    {
        MoveAimClampedToZone(Camera.main.ScreenToWorldPoint(value.Get<Vector2>()));
    }
#endregion

    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        SetupNewItem();
    }

    private void SetupNewItem()
    {
        currentItem = ItemSpawner.Instance.SpawnItem(aimPoint);
        currentItem.Freeze();
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