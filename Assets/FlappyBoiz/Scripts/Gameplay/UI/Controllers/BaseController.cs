using FlappyBoiz.Scripts.Gameplay.UI.Screens;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.UI.Controllers
{
    public class BaseController<T> : IPostInitializable
        where T : UIScreen
    {
        [Inject] protected T Screen { get; set; }

        internal void Show()
        {
            OnScreenInit();
            Screen.Show();
            OnScreenShow();
        }

        internal void Hide() => Screen.Hide();

        internal void Disable() => Screen.DisableInteraction();

        internal void Enable() => Screen.EnableInteraction();

        public void PostInitialize()
        {
            Hide();
        }
        
        protected virtual void OnScreenInit()
        {
        }

        protected virtual void OnScreenShow()
        {
        }
    }
}