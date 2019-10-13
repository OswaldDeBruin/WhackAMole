using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OzzysWhackAMole
{

    public class WAM_ScoreUIUpdater : MonoBehaviour
    {
        public WAM_GameManager GameManager;//The WhackAMole game
        public Text PlayerScoreTextElement;//The text informing us about the player's current score
        public Text HighScoreTextElement;//The all time high score

        

        //When this script is enabled, it means this screen has popped up and we need to update its info
        private void OnEnable()
        {
            //If we forgot to set the game manager, we find one ourself
            if (GameManager == null) GameManager = GameObject.FindObjectOfType<WAM_GameManager>();
            
            //If we have a game manager and the right UI elements, we can transfer the scores on screen
            if (GameManager != null && PlayerScoreTextElement != null)
            {
                PlayerScoreTextElement.text = "Your score this game was: " + GameManager.PlayerScore;
            }
            if (HighScoreTextElement != null)
            {
                HighScoreTextElement.text = "The overall high-score currently is: " + WAM_ScoreService.GetHighScore();
            }
        }
    }
}