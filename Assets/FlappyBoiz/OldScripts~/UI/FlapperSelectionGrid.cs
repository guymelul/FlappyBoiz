using FlappyBoiz.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GridLayoutGroup))]
    [RequireComponent(typeof(GameEventInvoker))]
    public class FlapperSelectionGrid : MonoBehaviour
    {
        [SerializeField]
        private GameSettings gameSettings;

        [SerializeField]
        private GameObject gridItem;

        private GameEventInvoker m_gameEvent;

        private FlapperSelectorGridItem[] m_flapperSelectors;

        private int EquippedFlapper => gameSettings.EquippedFlapper.Value;

        private void Start()
        {
            m_gameEvent = GetComponent<GameEventInvoker>();

            // Remove existing buttons
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            int index = 0;

            m_flapperSelectors = new FlapperSelectorGridItem[gameSettings.Flappers.Count];

            foreach (var flapper in gameSettings.Flappers)
            {
                var newGridItem = Instantiate(gridItem, transform);
                var flapperSelector = newGridItem.GetComponent<FlapperSelectorGridItem>();

                m_flapperSelectors[index] = flapperSelector;

                flapperSelector.SetFlapper(index, flapper.Preview, flapper.Price);
                flapperSelector.OnFlapperSelected += OnFlapperSelected;
                RefreshFlapper(index);

                index++;
            }

            gameSettings.FlapperUnlocks.Added.Register(RefreshFlapper);
            gameSettings.FlapperUnlocks.Removed.Register(RefreshFlapper);
        }

        public void RefreshAll()
        {
            Debug.Log($"Refreshing shop with {gameSettings.FlapperUnlocks.Count} / {gameSettings.Flappers.Count}" +
                      " unlocked flappers");

            for (int i = 0; i < gameSettings.Flappers.Count; i++)
            {
                RefreshFlapper(i);
            }
        }

        private void RefreshFlapper(int index)
        {
            m_flapperSelectors[index].SetState(
                locked: gameSettings.IsFlapperLocked(index),
                equipped: index == EquippedFlapper,
                (float)gameSettings.Coins.Value / Mathf.Max(1, gameSettings.Flappers[index].Price)
            );
        }

        private void OnFlapperSelected(int index)
        {
            // Check cell lock state
            if (gameSettings.IsFlapperLocked(index))
            {
                if (gameSettings.TryUnlockFlapper(index))
                {
                    // TODO: show buy animation
                }
                else
                {
                    PopupMessage.Instance.Show("You broke...");
                }
            }
            else
            {
                int oldEquippedFlapper = EquippedFlapper;

                gameSettings.SwitchFlapper(index);

                RefreshFlapper(oldEquippedFlapper);
                RefreshFlapper(index);

                // Close model
                m_gameEvent.Back();
            }
        }
    }
}