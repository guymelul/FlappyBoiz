using System.Collections.Generic;
using FlappyBoiz.Scripts.Features.Gameplay.Obstacle;
using UnityEngine;

namespace FlappyBoiz.Scripts.Features.Gameplay.Data
{
    [CreateAssetMenu(fileName = "ObstacleSpawnerSettings", menuName = "FlappyBoiz/Obstacle Spawner Settings",
        order = 0)]
    public class ObstacleSpawnerSettings : ScriptableObject
    {
        [field: SerializeField, Range(0.1f, 10.0f)]
        public float SpawnInterval { get; set; }

        [field: SerializeField, Range(0.1f, 50.0f)]
        public float ObstacleVelocity { get; set; }

        [field: SerializeField, Range(0f, 1000f)]
        public int MaxDifficultyScore { get; set; }

        [field: SerializeField] public AnimationCurve VariationCurve { get; set; }

        [field: SerializeField] public AnimationCurve DifficultyCurve { get; set; }

        [field: SerializeField] public List<BaseObstacle> Obstacles { get; set; }
    }
}