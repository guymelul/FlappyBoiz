using System.Collections.Generic;
using System.Linq;
using FlappyBoiz.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Gameplay.Obstacle
{
    public class ObstacleSpawnerView : MonoBehaviour
    {
        [SerializeField]
        private Vector2 movementDirection;

        [Inject] private ObstacleSpawnerSettings ObstacleSpawnerSettings { get; set; }

        public bool LastSpawnOutOfArea { get; private set; }

        private List<ObstaclePool<BaseObstacle>> m_obstaclePools;

        private void Start()
        {
            LastSpawnOutOfArea = true;

            m_obstaclePools = ObstacleSpawnerSettings.Obstacles
                .Select(obstaclePrototype =>
                    new ObstaclePool<BaseObstacle>(
                        obstaclePrototype,
                        transform,
                        onDestroy: Destroy,
                        maxSize: 4
                    )
                ).ToList();
        }

        public void SpawnObstacle(float variation, float difficulty)
        {
            // Pick random obstacle (pool)
            int objectPoolIdx = Random.Range(0, m_obstaclePools.Count - 1);
            var randObstaclePool = m_obstaclePools[objectPoolIdx];

            // Get from pool
            var obstacleGameObject = randObstaclePool.Spawn(variation, difficulty);

            // Init and reset position
            var motion = obstacleGameObject.GetComponent<ObstacleMotion>();
            obstacleGameObject.transform.localPosition = Vector3.zero;
            motion.Init(
                poolID: objectPoolIdx,
                speed: ObstacleSpawnerSettings.ObstacleVelocity,
                direction: movementDirection,
                onRelease: OnObstacleRelease
            );

            LastSpawnOutOfArea = false;
        }
        
        private void OnObstacleRelease(ObstacleMotion obstacle)
        {
            m_obstaclePools[obstacle.PoolID].Release(obstacle.gameObject);
        }
        
        private void OnTriggerExit2D(Collider2D other) => LastSpawnOutOfArea = true;

        private void OnDestroy()
        {
            foreach (var obstaclePool in m_obstaclePools)
            {
                obstaclePool.Clear();
            }
        }
    }
}