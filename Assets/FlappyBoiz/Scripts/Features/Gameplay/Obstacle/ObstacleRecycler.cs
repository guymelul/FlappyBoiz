using UnityEngine;

namespace FlappyBoiz.Scripts.Features.Gameplay.Obstacle
{
    [RequireComponent(typeof(Collider2D))]
    public class ObstacleRecycler : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            var obstacle = other.GetComponent<ObstacleMotion>();

            if (obstacle == null) return;

            obstacle.Release();
        }
    }
}