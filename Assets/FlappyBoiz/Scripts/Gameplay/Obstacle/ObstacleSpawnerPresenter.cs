using FlappyBoiz.Scripts.Core;
using FlappyBoiz.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.Obstacle
{
    public class ObstacleSpawnerPresenter : GameplayStateAware, IStartable, IFixedTickable
    {
        [Inject] private ObstacleSpawnerSettings ObstacleSpawnerSettings { get; set; }

        [Inject] private ObstacleSpawnerView ObstacleSpawnerView { get; set; }

        private float SpawnTimer { get; set; }

        // TODO: Mathf.Clamp01(score.Value / (float)(maxScoreThreshold * 2));
        // How wide is the gap
        private float Difficulty => 0;

        // How far from the center
        private float Variation => 0f;

        public void Start()
        {
            SpawnTimer = 0;
        }

        public void FixedTick()
        {
            if (!IsGameStarted) return;

            if (SpawnTimer > 0)
            {
                SpawnTimer -= Time.fixedDeltaTime;

                return;
            }

            if (!ObstacleSpawnerView.LastSpawnOutOfArea) return;

            Spawn();
        }

        private void Spawn()
        {
            // Ask spawner view to actually put obstacles in the game
            ObstacleSpawnerView.SpawnObstacle(Variation, Difficulty);

            // Restart timer
            SpawnTimer = ObstacleSpawnerSettings.SpawnInterval;
        }
    }
}