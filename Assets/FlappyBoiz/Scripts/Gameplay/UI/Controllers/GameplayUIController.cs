using FlappyBoiz.Scripts.Events;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.UI.Controllers
{
    public class GameplayUIController : GameplayStateAware, IStartable
    {
        [Inject] private ReadyController ReadyController { get; set; }
        [Inject] private PauseDialogController PauseDialogController { get; set; }
        [Inject] private GameOverController GameOverController { get; set; }
        [Inject] private HUDController HUDController { get; set; }
        
        // TODO: use stack for management

        public void Start()
        {
            ReadyController.Show();
        }

        protected override void OnGameStarted(GameStartedEvent ev)
        {
            ReadyController.Hide();
            HUDController.Show();
        }
        
        protected override void OnGamePaused(GamePausedEvent ev)
        {
            HUDController.Disable();
            PauseDialogController.Show();
        }

        protected override void OnGameResumed(GameResumedEvent ev)
        {
            PauseDialogController.Hide();
            HUDController.Enable();
        }

        protected override void OnGameEnded(GameEndedEvent ev)
        {
            HUDController.Hide();
            GameOverController.Show();
        }
    }
}