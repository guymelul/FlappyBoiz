using FlappyBoiz.Scripts.Core.UI;
using FlappyBoiz.Scripts.Events;
using FlappyBoiz.Scripts.Gameplay.Managers;
using FlappyBoiz.Scripts.Gameplay.UI.Screens;
using MessagePipe;
using R3;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.UI.Controllers
{
    public class HUDController : BaseController<HUDScreen>, IStartable
    {
        [Inject] private IPublisher<GamePauseCommand> GamePauseCommandPub { get; set; }

        [Inject] private GameSessionManager GameSessionManager { get; set; }

        public void Start()
        {
            GameSessionManager.Score.Subscribe(score => Screen.SetScore(score));

            Screen.OnPauseButtonClick = OnPause;
        }

        private void OnPause()
        {
            GamePauseCommandPub.Publish(GamePauseCommand.Empty);
        }
    }
}