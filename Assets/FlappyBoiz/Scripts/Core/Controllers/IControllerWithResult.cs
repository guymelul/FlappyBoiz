namespace FlappyBoiz.Scripts.Core.Controllers
{
    public interface IControllerWithResult<TResult> : IController
    {
        TResult Result { get; }
    }
}