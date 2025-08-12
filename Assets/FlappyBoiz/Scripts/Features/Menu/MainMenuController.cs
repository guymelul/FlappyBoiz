using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
using UnityEngine;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Menu
{
    public class MainMenuController : ControllerBase<MainMenuChoiceResult>
    {
        private readonly MainMenuScreen m_screen;

        public MainMenuController(IObjectResolver container,
            MainMenuScreen screen) : base(container)
        {
            m_screen = screen;
        }

        protected override async UniTask<MainMenuChoiceResult> OnStartWithResultAsync(CancellationToken ct)
        {
            // TODO: add back button task, and other options
            try
            {
                Debug.Log("Started main menu");
                await UniTask.WhenAny(m_screen.StartButton.OnClickAsync(ct));
                return MainMenuChoiceResult.Play;
            }
            catch (OperationCanceledException)
            {
                return MainMenuChoiceResult.Quit;
            }
        }
    }
}