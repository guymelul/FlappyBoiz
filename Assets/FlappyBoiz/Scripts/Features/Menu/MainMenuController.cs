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
                await m_screen.Show(ct);

                Debug.Log("Started main menu");
                await m_screen.StartButton.OnClickAsync(ct);
                Debug.Log("Play clicked");

                await m_screen.Hide(ct);

                return MainMenuChoiceResult.Play;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Canceled main menu");

                return MainMenuChoiceResult.Quit;
            }
        }
    }
}