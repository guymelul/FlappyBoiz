using System;
using System.Collections.Generic;

namespace FlappyBoiz.Scripts.ScriptableObjects
{
    [Serializable]
    public class GameSavedData
    {
        public int coinsBalance;
        public int totalCoinsEarned;
        public int highScore;
        public int equippedFlapper;
        public int equippedTheme;
        public int equippedCosmetic;
        public List<int> flappersUnlocked = new(0);
        public List<int> themesUnlocked = new(0);
        public List<int> cosmeticsUnlocked = new(0);
    }
}