using frare.Vector3Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerInput playerInput;

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
            this.transform.position += moveSpeed * Time.deltaTime * moveInput;
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

    private void SetupNewItem()
    {
        currentItem = ItemSpawner.Instance.SpawnItem(this.transform);
        currentItem.Freeze();
        currentItem.onFinishedMoving += OnItemFinishedMoving;
    }

    private void OnItemFinishedMoving()
    {
        if (currentItem != null) currentItem.onFinishedMoving -= OnItemFinishedMoving;
        currentItem = null;
        SetupNewItem();
    }
}