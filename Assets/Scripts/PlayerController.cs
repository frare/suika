using UnityEngine;
using UnityEngine.InputSystem;
using frare.Vector3Extensions;
using frare.CoroutineHelper;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float shootCooldown;
    [SerializeField] private Transform aimPoint;
    [SerializeField] private BoxCollider2D aimZone;
    [SerializeField] private BoxCollider2D gameArea;
    [SerializeField] private float moveSpeed;

    private float AimRadiusOffset
    {
        get
        {
            if (currentItem == null) return 0f;
            else return currentItem.Radius + 0.05f; // add a small offset so the items don't bump into the lateral walls
        }
    }

    private Coroutine shootTimerCoroutine;
    private Vector2 pointerInput;
    private Item currentItem;

#region UNITY CALLBACKS
    private void Start()
    {
        SetupNewItem();
    }
#endregion

#region INPUT SYSTEM CALLBACKS
    public void OnPointer(InputValue value) // mouse and touch
    {
        if (GameManager.Instance.IsPaused) return;

        pointerInput = value.Get<Vector2>();
        MoveAimClampedToZone(Camera.main.ScreenToWorldPoint(pointerInput));
    }

    public void OnInteractPointer(InputValue value) // mouse and touch Click
    {
        if (GameManager.Instance.IsPaused) return;

        if (currentItem != null && IsPointerOnGameArea(pointerInput)) // check for game area to avoid UI clicks triggering the item release
        {
            currentItem.Release();
            currentItem = null;
            CoroutineHelper.StartCoroutineOverride(this, ref shootTimerCoroutine, ShotCooldown());
        }
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
        MoveAimClampedToZone(Camera.main.ScreenToWorldPoint(pointerInput));
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

    private bool IsPointerOnGameArea(Vector2 screenPosition)
    {
        if (IsPointerOverUI(screenPosition)) return false;

        var worldPointerPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPointerPosition.z = 0f;

        return gameArea.bounds.Contains(worldPointerPosition);
    }

    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return false;

        PointerEventData pointerEventData = new(EventSystem.current)
        {
            position = screenPosition
        };
        List<RaycastResult> raycastResults = new();

        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        foreach (var result in raycastResults)
        {
            if (result.module is GraphicRaycaster) return true;
        }

        return false;
    }
}