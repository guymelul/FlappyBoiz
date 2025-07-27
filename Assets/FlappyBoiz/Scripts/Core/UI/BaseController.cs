using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
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

        protected async UniTask<Button> ButtonSelector(CancellationToken cancellationToken, params Button[] buttons)
        {
            // Handle the edge case where no buttons are provided.
            if (buttons == null || buttons.Length == 0)
            {
                throw new Exception("No buttons passed to ButtonSelector");
            }
            
            var clickTasks = buttons.Select(button => button.OnClickAsync(cancellationToken)).ToArray();

            // UniTask.WhenAny awaits all tasks in the array and returns the index of the first one to complete.
            int completedIndex = await UniTask.WhenAny(clickTasks);

            // Return the button that corresponds to the completed task.
            return buttons[completedIndex];
        }
    }
}