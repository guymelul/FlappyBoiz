using System;
using UnityEngine;
using UnityEngine.Pool;

namespace FlappyBoiz.Scripts.Gameplay.Obstacle
{
    internal class ObstaclePool<T> where T : BaseObstacle
    {
        private readonly ObjectPool<GameObject> m_objectPool;
        private readonly T m_prototype;
        private readonly Transform m_spawnLocation;

        public ObstaclePool(
            T prototype,
            Transform spawnLocation,
            Action<GameObject> onDestroy,
            int defaultCapacity = 2,
            int maxSize = 5)
        {
            m_spawnLocation = spawnLocation;
            m_prototype = prototype;

            m_objectPool = new ObjectPool<GameObject>(
                createFunc: CreateObstacle,
                actionOnGet: OnSpawn,
                actionOnRelease: OnRelease,
                actionOnDestroy: onDestroy,
                maxSize: maxSize,
                defaultCapacity: defaultCapacity
            );
        }

        public GameObject Spawn(float variation = 0.0f, float difficulty = 0.0f)
        {
            var obstacleGameObject = m_objectPool.Get();

            m_prototype.Randomize(obstacleGameObject, variation, difficulty);

            return obstacleGameObject;
        }

        public void Release(GameObject obstacleGameObject) => m_objectPool.Release(obstacleGameObject);

        public void Clear() => m_objectPool.Clear();

        private void OnSpawn(GameObject obstacleGameObject)
        {
            obstacleGameObject.transform.SetParent(m_spawnLocation, false);
            obstacleGameObject.SetActive(true);
        }

        private GameObject CreateObstacle()
        {
            var obstacleGameObject = m_prototype.Create();
            obstacleGameObject.SetActive(false);
            obstacleGameObject.name = $"PooledObstacle #{m_objectPool.CountActive}";

            return obstacleGameObject;
        }

        private static void OnRelease(GameObject obstacleGameObject)
        {
            obstacleGameObject.SetActive(false);
        }
    }
}