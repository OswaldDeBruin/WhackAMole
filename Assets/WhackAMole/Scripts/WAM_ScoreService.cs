using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OzzysWhackAMole
{
    //A simple service remembering our scores, which we can expand upon when needed.
    public static class WAM_ScoreService
    {
        const string HighScoreKey = "LocalHighScore";//The key we use in for the Highscore in Unity player prefs

        //This function is called by the gamemanager when a game has ended
        public static void SubmitScore(int playerScore)
        {
            if (playerScore > GetHighScore())//Only remember the score when it is a high score
            {
                PlayerPrefs.SetInt(HighScoreKey,playerScore);
            }
        }

        //publicly available function to get the highscore when needed
        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt(HighScoreKey, 0);//Returns Highscore key value if it exists, if not, the high score is 0
        }
    }
}