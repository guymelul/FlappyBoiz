using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.Obstacle
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObstacleMotion : MonoBehaviour
    {
        //[SerializeField] private FloatReference obstacleSpeed;
        
        // Pool leasing ID
        internal int LeaseID { get; private set; }

        private Rigidbody2D m_rigidbody2D;

        private void Awake()
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
        }

        internal void Move(int leaseID, Vector2 direction)
        {
            LeaseID = leaseID;
            //m_rigidbody2D.linearVelocity = direction * obstacleSpeed.Value;
        }
    }
}