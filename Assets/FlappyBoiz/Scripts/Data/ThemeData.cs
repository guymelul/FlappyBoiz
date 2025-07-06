using UnityEngine;

namespace FlappyBoiz.Scripts.Data
{
    [CreateAssetMenu(fileName = "theme", menuName = "FlappyBoiz/Theme", order = 0)]
    public class ThemeData : ScriptableObject
    {
        [field: SerializeField] public Sprite LightBackground { get; set; }

        [field: SerializeField] public Sprite DarkBackground { get; set; }

        [field: SerializeField] public Sprite Ground { get; set; }

        [field: SerializeField] public SoundPackData SoundPack { get; set; }
    }
}