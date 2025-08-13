using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.ScriptableObjects;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Gameplay.Obstacle
{
    public class ObstacleSpawnerLogic : GameplayStateAware, IStartable, IFixedTickable
    {
        [Inject] public ObstacleSpawnerSettings ObstacleSpawnerSettings { get; set; }

        [Inject] public ObstacleSpawnerPresenter ObstacleSpawnerPresenter { get; set; }

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

        protected override void OnGameEnded(GameEndedEvent ev)
        {
            ObstacleSpawnerPresenter.Clear();
        }

        public void FixedTick()
        {
            if (!IsGameRunning) return;

            if (SpawnTimer > 0)
            {
                SpawnTimer -= Time.fixedDeltaTime;

                return;
            }

            if (!ObstacleSpawnerPresenter.LastSpawnOutOfArea) return;

            Spawn();
        }

        private void Spawn()
        {
            // Ask spawner view to actually put obstacles in the game
            ObstacleSpawnerPresenter.SpawnObstacle(Variation, Difficulty);

            // Restart timer
            SpawnTimer = ObstacleSpawnerSettings.SpawnInterval;
        }
    }
}