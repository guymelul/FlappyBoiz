using FlappyBoiz.Scripts.Events;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Gameplay.Presenters
{
    public class GameplayUIController : GameplayStateAware, IStartable
    {
        [Inject] public ReadyPresenter ReadyPresenter { get; set; }
        [Inject] public PauseDialogPresenter PauseDialogPresenter { get; set; }
        [Inject] public GameOverPresenter GameOverPresenter { get; set; }
        [Inject] public HUDPresenter HUDPresenter { get; set; }

        // TODO: use stack for management

        public void Start()
        {
            ReadyPresenter.Show();
        }

        protected override void OnGameStarted(GameStartedEvent ev)
        {
            ReadyPresenter.Hide();
            HUDPresenter.Show();
        }

        protected override void OnGamePaused(GamePausedEvent ev)
        {
            HUDPresenter.Disable();
            PauseDialogPresenter.Show();
        }

        protected override void OnGameResumed(GameResumedEvent ev)
        {
            PauseDialogPresenter.Hide();
            HUDPresenter.Enable();
        }

        protected override void OnGameEnded(GameEndedEvent ev)
        {
            HUDPresenter.Hide();
            GameOverPresenter.Show();
        }
    }
}