using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    public interface IController : IDisposable
    {
        IReadOnlyList<IController> Children { get; }

        UniTask InitAsync(CancellationToken ct);

        UniTask StartAsync(CancellationToken ct);

        UniTask StopAsync(CancellationToken ct);
    }

}