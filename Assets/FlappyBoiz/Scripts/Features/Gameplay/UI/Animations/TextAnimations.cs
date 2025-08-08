using LitMotion;
using LitMotion.Extensions;
using TMPro;

namespace FlappyBoiz.Scripts.Features.Gameplay.UI.Animations
{
    public static class TextAnimations
    {
        public static MotionHandle NumericTextMotion(TMP_Text textComponent, int from, int to, float duration = 1f)
        {
            return LMotion.Create(from, to, duration)
                .WithEase(Ease.InOutCubic)
                .BindToText(textComponent, "{0:d}");
        }
    }
}