using UnityEngine;

namespace FlappyBoiz.Scripts.Features.Gameplay.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FlapperSettings", menuName = "FlappyBoiz/Flapper Settings", order = 0)]
    public class FlapperSettings : ScriptableObject
    {
        [field: Range(0.0f, 20.0f), SerializeField]
        public float MaxFallSpeed { get; set; }

        [field: Range(0.0f, 50.0f), SerializeField]
        public float FallAcceleration { get; set; }

        [field: Range(1.0f, 5.0f), SerializeField]
        public float FlapPower { get; set; }

        [field: Range(0.0f, 0.5f), SerializeField]
        public float FlapCooldown { get; set; }

        [field: Range(0, 90), SerializeField] public int JumpRotation { get; set; }
        [field: Range(0, 90), SerializeField] public int AirborneTargetRotation { get; set; }
        [field: Range(-90, 0), SerializeField] public int FallMaxRotation { get; set; }

        [field: Range(1, 360), SerializeField] public int FallRotationSpeed { get; set; }
    }
}