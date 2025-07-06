using FlappyBoiz.Scripts.Managers;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    public class GameEventInvoker : MonoBehaviour
    {
        [SerializeField]
        private StringEvent gameEvent;

        private void Start()
        {
            if (gameEvent == null)
                Debug.LogError("Game Event Bus is not set!");
        }

        private void Raise(string eventName) => gameEvent.Raise(eventName);

        public void Action() => Raise(GameEvents.Action);

        public void Back() => Raise(GameEvents.UIBack);

        public void Exit() => Raise(GameEvents.Exit);

        public void Restart() => Raise(GameEvents.Restart);

        public void GameOver() => Raise(GameEvents.GameOver);
        
        public void Authenticated() => Raise(GameEvents.Authenticated);
        
        public void OperationSuccess() => Raise(GameEvents.OperationSuccess);
    }
}