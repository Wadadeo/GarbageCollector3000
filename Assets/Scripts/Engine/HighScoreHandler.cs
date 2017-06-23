using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEngine
{
    public static class HighScoreHandler
    {
        public static readonly string highScoreToken = "HighScore";

        /// <summary>
        /// Get the current game high score
        /// </summary>
        /// <returns>The high score value</returns>
        public static int GetCurrentHighScore()
        {
            return PlayerPrefs.GetInt(highScoreToken, 0);
        }
        
        /// <summary>
        /// Manage the new score, storing it if new high score
        /// </summary>
        /// <param name="newScore">The new score to process</param>
        /// <returns>True if new high score, false if not</returns>
        public static bool HandleNewScore(int newScore)
        {
            if (GetCurrentHighScore() < newScore)
            {
                PlayerPrefs.SetInt(highScoreToken, newScore);
                return true;
            }
            return false;
        }
    }
}
