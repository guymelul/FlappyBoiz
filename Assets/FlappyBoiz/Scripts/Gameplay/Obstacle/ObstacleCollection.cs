using System.Collections.Generic;
using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.Obstacle
{
    [CreateAssetMenu(menuName ="FlappyBoiz/Obstacle Collection")]
    public class ObstacleCollection: ScriptableObject
    {
        [SerializeField]
        private List<BaseObstacle> obstacles;
        
        public List<BaseObstacle> Obstacles => obstacles;

        public int Count => obstacles.Count;
    }
}