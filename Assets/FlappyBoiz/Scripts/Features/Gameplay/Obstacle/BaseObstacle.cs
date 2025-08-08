using UnityEngine;

namespace FlappyBoiz.Scripts.Features.Gameplay.Obstacle
{
    public abstract class BaseObstacle : ScriptableObject
    {
        public abstract GameObject Create();

        public abstract void Randomize(GameObject gameObject, float variation, float difficulty);
    }
}