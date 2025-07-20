using UnityEngine;

namespace FlappyBoiz.Scripts.Gameplay.ScriptableObjects
{
    [CreateAssetMenu(fileName = "flapper", menuName = "FlappyBoiz/Flapper", order = 0)]
    public class FlapperData : ScriptableObject
    {
        [field: SerializeField] public int Price { get; set; }
        
        [field: SerializeField] public Sprite Preview { get; set; }

        [field: SerializeField] public AnimatorOverrideController AnimatorOverride { get; set; }
    }
}