using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Features.Gameplay.Managers;
using FlappyBoiz.Scripts.Features.Gameplay.ScriptableObjects;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Gameplay.Obstacle
{
    public class ObstacleSpawnerLogic : GameplayStateAware
    {
        [Inject] public ObstacleSpawnerSettings ObstacleSpawnerSettings { get; set; }

        [Inject] public ObstacleSpawnerPresenter ObstacleSpawnerPresenter { get; set; }

        [Inject] public GameSessionManager GameSessionManager { get; set; }

        private float SpawnTimer { get; set; }

        private int CurrentScore => GameSessionManager.Score.CurrentValue;
        private float Progress => Mathf.Clamp01(CurrentScore / (float)ObstacleSpawnerSettings.MaxDifficultyScore);

        private float Difficulty => ObstacleSpawnerSettings.DifficultyCurve.Evaluate(Progress);
        private float Variation => ObstacleSpawnerSettings.VariationCurve.Evaluate(Progress);

        protected override void OnGameStarted(GameStartedEvent ev)
        {
            SpawnTimer = 0;
        }

        protected override void OnGameEnded(GameEndedEvent ev)
        {
            ObstacleSpawnerPresenter.Clear();
        }

        protected override void OnGameFixedTick()
        {
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
            Debug.Log($"Spawn [progress={Progress}, diff={Difficulty}, var={Variation}]");
            ObstacleSpawnerPresenter.SpawnObstacle(Variation, Difficulty);

            // Restart timer
            SpawnTimer = ObstacleSpawnerSettings.SpawnInterval;
        }
    }
}