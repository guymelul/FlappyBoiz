using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace FlappyBoiz.Scripts
{
    public static class Bootstrap
    {
        [RuntimeInitializeOnLoadMethod]
        public static void LimitFrameRate()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 61;

            EnhancedTouchSupport.Enable();
        }
    }
}