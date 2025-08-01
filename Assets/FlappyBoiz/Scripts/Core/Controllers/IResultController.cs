using System.Threading;
using Cysharp.Threading.Tasks;

namespace FlappyBoiz.Scripts.Core.Controllers
{
    /// <summary>
    /// Represents a controller that executes a flow and returns a result.
    /// Ideal for popups, wizards, or any modal user interaction.
    /// </summary>
    /// <typeparam name="TResult">The type of the result this controller will return.</typeparam>
    public interface IResultController<TResult> : IController
    {
        /// <summary>
        /// RUn the controller's view and awaits a user interaction or process completion.
        /// </summary>
        /// <returns>A UniTask containing the result.</returns>
        UniTask<TResult> RunAsync(CancellationToken ct);
    }
}