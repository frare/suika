using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null && collision.attachedRigidbody.TryGetComponent(out Item item))
        {
            GameManager.Instance.GameOver();
        }
    }
}