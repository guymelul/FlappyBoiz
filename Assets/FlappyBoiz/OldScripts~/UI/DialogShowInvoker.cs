using FlappyBoiz.Scripts.Managers;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    public class DialogShowInvoker : MonoBehaviour
    {
        [SerializeField] private StringEvent dialogShowEvent;

        private void Start()
        {
            if (dialogShowEvent == null)
                Debug.LogError("Dialog Show Bus is not set!");
        }

        private void Raise(string eventName) => dialogShowEvent.Raise(eventName);

        public void ShowSkinSelectionDialog() => Raise(Dialogs.SkinSelection);

        public void ShowSettingsDialog() => Raise(Dialogs.Settings);
        
        public void ShowLeaderboardDialog() => Raise(Dialogs.Leaderboard);
    }
}