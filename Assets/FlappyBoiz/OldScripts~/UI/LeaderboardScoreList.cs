using System.Collections.Generic;
using System.Linq;
using FlappyBoiz.Scripts.Managers;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    public class LeaderboardScoreList : MonoBehaviour
    {
        [SerializeField]
        private GameObject scoreListItemPrefab;

        private GameEventInvoker m_gameEventInvoker;

        private List<GameObject> m_scoreListItems;

        private TMP_Text m_message;

        private const int PreloadedSize = 25;

        private void Awake()
        {
            m_gameEventInvoker = GetComponent<GameEventInvoker>();
            m_message = transform.GetChild(0).GetComponent<TMP_Text>();
        }

        private void Start()
        {
            // Remove existing buttons
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child != m_message.gameObject)
                    Destroy(child);
            }

            // Default is 25 on GPGS

            float itemHeight = scoreListItemPrefab.GetComponent<RectTransform>().rect.height;

            m_scoreListItems = new List<GameObject>(PreloadedSize);

            for (int i = 0; i < PreloadedSize; i++)
            {
                var listItemGameObject = Instantiate(scoreListItemPrefab, transform);
                listItemGameObject.SetActive(false);

                listItemGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, itemHeight * -i);

                m_scoreListItems.Add(listItemGameObject);
            }

            // TODO: initialize a pool of item objects, disabled

            // TODO: show load state, add timeout and popup message and refresh button

            // TODO: on close. clear the list and switch back to load state
        }

        public void HideAllItems()
        {
            if (m_scoreListItems is not { Count: > 0 }) return;

            foreach (var item in m_scoreListItems)
            {
                item.SetActive(false);
            }
        }

        public void LoadScores()
        {
            if (Social.localUser.authenticated)
            {
                m_message.text = "Loading Scores...";
                string leaderboardId = GameDataManager.GetLeaderboardId();
                Debug.Log($"Loading leaderboard {leaderboardId}");

                if (PlayGamesPlatform.Instance.IsAuthenticated())
                {
                    m_gameEventInvoker.Back();
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
                }
                else
                {
                    Social.LoadScores(leaderboardId, OnScoresLoaded);
                }
            }
            else
            {
                PopupMessage.Instance.Show("Login to show leaderboard");
                m_gameEventInvoker.Back();
            }
        }

        private void OnScoresLoaded(IScore[] scores)
        {
            if (scores == null || scores.Length == 0)
            {
                m_message.text = "No Scores Found";

                return;
            }

            if (scores.Length > PreloadedSize)
            {
                Debug.LogError($"Loaded {scores.Length} leaderboard scores instead of {PreloadedSize}");
            }

            int scoresToShow = Mathf.Min(PreloadedSize, scores.Length);

            string[] userIDs = scores.Select(score => score.userID).ToArray();

            Social.LoadUsers(userIDs, userProfiles =>
            {
                if (userProfiles == null || userProfiles.Length == 0)
                {
                    m_message.text = "Failed to load leaderboard";

                    return;
                }

                var userProfileMap = userProfiles.ToDictionary(u => u.id, u => u);

                m_message.text = "";

                for (int i = 0; i < scoresToShow; i++)
                {
                    var score = scores[i];
                    string userID = score.userID;

                    var listItem = m_scoreListItems[i];
                    var listItemView = listItem.GetComponent<LeaderboardScoreListItem>();

                    string username = userID == Social.localUser.id
                        ? Social.localUser.userName
                        : userProfileMap.GetValueOrDefault(userID).userName;

                    listItemView.SetInfo(score.rank, username, score.value);

                    listItem.SetActive(true);
                }
            });
        }
    }
}