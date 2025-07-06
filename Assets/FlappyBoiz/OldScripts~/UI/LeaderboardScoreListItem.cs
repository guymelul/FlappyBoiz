using TMPro;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    public class LeaderboardScoreListItem : MonoBehaviour
    {
        private TMP_Text m_rank;
        private TMP_Text m_username;
        private TMP_Text m_score;

        private void Awake()
        {
            var textFields = GetComponentsInChildren<TMP_Text>();
            m_rank = textFields[0];
            m_username = textFields[1];
            m_score = textFields[2];
        }

        public void SetInfo(int rank, string username, long score)
        {
            m_rank.text = $"#{rank}";
            m_username.text = username;
            m_score.text = score.ToString();
        }
    }
}