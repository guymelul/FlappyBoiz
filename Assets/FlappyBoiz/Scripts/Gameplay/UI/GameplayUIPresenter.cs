using FlappyBoiz.Scripts.Core;
using FlappyBoiz.Scripts.Events;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.UI
{
    public class GameplayUIPresenter : GameplayStateAware, IStartable
    {
        [Inject] private GameplayUIView GameplayUIView { get; set; }

        public void Start()
        {
            GameplayUIView.ShowReadyScreen();
        }

        protected override void OnGameStarted(GameStartedEvent ev)
        {
            GameplayUIView.ShowHUDScreen();
        }

        protected override void OnGameResumed(GameResumedEvent ev)
        {
            GameplayUIView.ShowHUDScreen();
        }

        protected override void OnGamePaused(GamePausedEvent ev)
        {
            GameplayUIView.ShowPauseOverlay();
        }
    }
}