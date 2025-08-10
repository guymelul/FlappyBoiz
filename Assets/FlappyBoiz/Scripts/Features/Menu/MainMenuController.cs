using System.Threading;
using Cysharp.Threading.Tasks;
using FlappyBoiz.Scripts.Core.Controllers;
using VContainer;

namespace FlappyBoiz.Scripts.Features.Menu
{
    public class MainMenuController : ControllerBase
    {
        public MainMenuController(IObjectResolver container) : base(container)
        {
        }

        protected override async UniTask OnStartAsync(CancellationToken ct)
        {
        }
    }
}