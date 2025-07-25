using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core.UI
{
    public class BaseController<T> : IPostInitializable
        where T : UIScreen
    {
        [Inject] protected T Screen { get; set; }

        public void Show()
        {
            OnScreenInit();
            Screen.Show();
            OnScreenShow();
        }

        public void Hide() => Screen.Hide();

        public void Disable() => Screen.DisableInteraction();

        public void Enable() => Screen.EnableInteraction();

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
        
        protected virtual void OnScreenHide()
        {
        }
    }
}