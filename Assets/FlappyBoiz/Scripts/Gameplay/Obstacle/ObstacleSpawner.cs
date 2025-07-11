using System.Collections.Generic;
using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.Obstacle
{
    public class ObstacleSpawner : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Events and Variables")]
        // [SerializeField]
        // private GameObjectEvent obstacleCanRecycleEvent;
        //
        // [SerializeField]
        // private StringEvent gameStateChanged;
        //
        // [SerializeField]
        // private IntReference score;
        //
        // [SerializeField]
        // private FloatReference obstacleSpeed;

        [SerializeField]
        private ObstacleCollection obstacleCollection;

        [Header("Spawn settings")]
        [SerializeField]
        private Vector2 movementDirection;

        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float spawnInterval;

        [SerializeField]
        [Range(1, 1000)]
        private int maxScoreThreshold;

        [SerializeField]
        private AnimationCurve difficultyCurve;

        #endregion

        #region Members

        private bool m_shouldSpawn;
        private float m_spawnTimer;
        private bool m_lastSpawnOutOfArea;
        private List<ObstaclePool<BaseObstacle>> m_obstaclePools;

        #endregion

        private void Awake()
        {
            m_lastSpawnOutOfArea = true;
            m_spawnTimer = 1;

            m_obstaclePools = new List<ObstaclePool<BaseObstacle>>(obstacleCollection.Count);

            foreach (var obstaclePrototype in obstacleCollection.Obstacles)
            {
                var pool = new ObstaclePool<BaseObstacle>(
                    obstaclePrototype,
                    transform,
                    onDestroy: Destroy
                );

                m_obstaclePools.Add(pool);
            }
        }

        private void OnEnable()
        {
            //gameStateChanged?.Register(OnGameStateChanged);
            //obstacleCanRecycleEvent.Register(OnObstacleCanRecycle);
        }

        private void OnDisable()
        {
            //gameStateChanged?.Unregister(OnGameStateChanged);
            //obstacleCanRecycleEvent.Unregister(OnObstacleCanRecycle);
        }
        
        // TODO: fix
        // private void OnGameStateChanged(string state)
        // {
        //     m_shouldSpawn = GameStates.Playing == state;
        //
        //     switch (state)
        //     {
        //         case GameStates.MainMenu:
        //         case GameStates.Ready:
        //         case GameStates.GameOver:
        //             m_obstaclePools.ForEach(pool => { pool.Clear(); });
        //
        //             for (int i = transform.childCount - 1; i >= 0; --i)
        //             {
        //                 var child = transform.GetChild(i).gameObject;
        //                 Destroy(child);
        //             }
        //
        //             break;
        //     }
        // }

        private void OnTriggerExit2D(Collider2D other) => m_lastSpawnOutOfArea = true;

        private void OnObstacleCanRecycle(GameObject obstacleGameObject)
        {
            var motion = obstacleGameObject.GetComponent<ObstacleMotion>();
            m_obstaclePools[motion.LeaseID].Release(obstacleGameObject);
            // Go back to start
            obstacleGameObject.transform.localPosition = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (!m_shouldSpawn) return;

            m_spawnTimer -= Time.deltaTime;

            if (!m_lastSpawnOutOfArea || m_spawnTimer > 0) return;

            SpawnNextObstacle();
        }

        private float MaxVariation => 0;// Mathf.Clamp01(difficultyCurve.Evaluate(score.Value / (float)maxScoreThreshold));

        private float Difficulty => 0;// Mathf.Clamp01(score.Value / (float)(maxScoreThreshold * 2));

        // TODO: another curve for variation
        private float NextVariation() => Random.Range(0.0f, 1.0f) * MaxVariation;

        private void SpawnNextObstacle()
        {
            // TODO: Pick random object pool
            int objectPoolIdx = Random.Range(0, m_obstaclePools.Count - 1);
            var randObstaclePool = m_obstaclePools[objectPoolIdx];

            // Get from pool
            var obstacleGameObject = randObstaclePool.Spawn(NextVariation(), Difficulty);

            var motion = obstacleGameObject.GetComponent<ObstacleMotion>();
            motion.Move(leaseID: objectPoolIdx, movementDirection);

            m_spawnTimer = spawnInterval;
            m_lastSpawnOutOfArea = false;
        }

        private void OnDestroy()
        {
            foreach (var obstaclePool in m_obstaclePools)
            {
                obstaclePool.Clear();
            }
        }
    }
}