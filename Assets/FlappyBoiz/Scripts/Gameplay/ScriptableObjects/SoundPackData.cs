using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.ScriptableObjects
{
    [CreateAssetMenu(fileName = "soundPack", menuName = "FlappyBoiz/SoundPack", order = 0)]
    public class SoundPackData : ScriptableObject
    {
        [field: SerializeField] public AudioClip Die { get; set; }

        [field: SerializeField] public AudioClip Hit { get; set; }

        [field: SerializeField] public AudioClip Point { get; set; }

        [field: SerializeField] public AudioClip WingFlap { get; set; }

        [field: SerializeField] public AudioClip BgmPlaying { get; set; }
        [field: SerializeField] public AudioClip BgmMenu { get; set; }

        [field: SerializeField] public AudioClip UISelect { get; set; }
        [field: SerializeField] public AudioClip UIUnavailable { get; set; }
    }
}