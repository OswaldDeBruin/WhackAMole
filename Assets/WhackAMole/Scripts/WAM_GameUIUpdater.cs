using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace OzzysWhackAMole
{
    //Class that updates the game UI every frame for an accurate representation of the game
    public class WAM_GameUIUpdater : MonoBehaviour
    {
        public WAM_GameManager GameManager;
        public Text TimeTextElement;
        public Text ScoreTextElement;

        void Start()
        {
            //if we forget to set the gamemanager, we find one on ourself
            if (GameManager == null)
            {
                GameManager = GameObject.FindObjectOfType<WAM_GameManager>();
            }
        }

        void Update()
        {
            //Only update if we've got the right objects
            if (GameManager == null) return;
            if (ScoreTextElement != null)
            {
                ScoreTextElement.text = "Score: " + GameManager.PlayerScore;
            }
            if (TimeTextElement != null)
            {
                TimeTextElement.text = "Time: " + Mathf.Ceil(GameManager.GetTimeLeft());
            }
        }
    }
}