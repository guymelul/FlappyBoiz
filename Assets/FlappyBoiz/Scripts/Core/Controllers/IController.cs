using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    public interface IController : IDisposable
    {
        /// <summary>
        /// A read-only list of the direct children of this controller.
        /// </summary>
        internal IReadOnlyList<IController> Children { get; }

        /// <summary>
        /// Initializes the controller. Used for setting up dependencies,
        /// subscribing to messages, and preparing resources.
        /// Does not start the controller's main logic.
        /// </summary>
        internal UniTask InitAsync(CancellationToken ct);

        /// <summary>
        /// Starts the controller's main logic and spawns any necessary child controllers.
        /// </summary>
        internal UniTask StartAsync(CancellationToken ct);

        /// <summary>
        /// Gracefully stops the controller's logic and the logic of all its children.
        /// </summary>
        UniTask StopAsync(CancellationToken ct);
    }
}