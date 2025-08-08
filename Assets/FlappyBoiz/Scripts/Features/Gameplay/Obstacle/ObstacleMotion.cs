using System;
using UnityEngine;

namespace FlappyBoiz.Scripts.Features.Gameplay.Obstacle
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObstacleMotion : MonoBehaviour
    {
        // Pool leasing ID
        internal int PoolID { get; private set; }

        private Rigidbody2D m_rigidbody2D;
        private Action<ObstacleMotion> m_onRelease;

        private void Awake()
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Init(int poolID, float speed, Vector2 direction, Action<ObstacleMotion> onRelease)
        {
            PoolID = poolID;
            m_rigidbody2D.linearVelocity = direction * speed;
            this.m_onRelease = onRelease;
        }

        public void Release()
        {
            m_onRelease?.Invoke(this);
            m_onRelease = null;
            m_rigidbody2D.linearVelocity = Vector2.zero;
        }
    }
}