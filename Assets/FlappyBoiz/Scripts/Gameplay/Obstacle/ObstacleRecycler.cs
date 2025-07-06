using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.Obstacle
{
    [RequireComponent(typeof(Collider2D))]
    public class ObstacleRecycler : MonoBehaviour
    {
        //[SerializeField] private GameObjectEvent obstacleCanRecycleEvent;

        private void OnTriggerExit2D(Collider2D other)
        {
            //obstacleCanRecycleEvent?.Raise(other.gameObject);
        }
    }
}