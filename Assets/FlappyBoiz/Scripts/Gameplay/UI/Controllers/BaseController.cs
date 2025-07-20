using FlappyBoiz.Scripts.Gameplay.UI.Screens;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.UI.Controllers
{
    public class BaseController<T> : IPostInitializable
        where T : UIScreen
    {
        [Inject] protected T Screen { get; set; }

        internal void Show() => Screen.Show();

        internal void Hide() => Screen.Hide();
        
        internal void Disable() => Screen.Disable();
        internal void Enable() => Screen.Enable();

        public void PostInitialize()
        {
            Hide();
        }
    }
}