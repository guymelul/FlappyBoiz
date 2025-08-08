using UnityEngine;

namespace FlappyBoiz.Scripts.Features.Gameplay.ScriptableObjects
{
    [CreateAssetMenu(fileName = "cosmetic", menuName = "FlappyBoiz/Cosmetics", order = 0)]
    public class CosmeticData : ScriptableObject
    {
        [field: SerializeField] public int Price { get; set; }

        [field: SerializeField] public Sprite Sprite { get; set; }
    }
}