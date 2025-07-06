using System.Threading;
using Cysharp.Threading.Tasks;

namespace FlappyBoiz.Scripts.Data
{
    public interface IGameDataStore<T> where T : class
    {
        UniTask Save(T gameSavedData, CancellationToken token);
        UniTask<T> Load(CancellationToken token);
    }
}