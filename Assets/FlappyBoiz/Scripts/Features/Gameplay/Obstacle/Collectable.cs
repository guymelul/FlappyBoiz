using UnityEngine;

namespace FlappyBoiz.Scripts.Features.Gameplay.Obstacle
{
    public class Collectable : MonoBehaviour
    {
        [field: SerializeField, Range(1, 100)] public int Amount { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            gameObject.SetActive(false);
        }
    }
}