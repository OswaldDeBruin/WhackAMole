using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OzzysWhackAMole
{
    //Game manager with state machine to handle the game
    public class WAM_GameManager : MonoBehaviour
    {
        enum GameState { idle, InGame, EndGame };//Game states
        GameState myState = GameState.idle;//Init state
        private List<WAM_MoleScript> MoleCollection = new List<WAM_MoleScript>();//All the moles in the WhackAmole game
        public float GameTimeLength = 30;//The time a game round should take
        float GameStartTime = 0;//The moment the game round starts

        public float PatternTransitionTime = 3;//Time in between mole patterns
        float LastPatternTransitionMoment = 0;//The last moment we switched between mole patterns

        public int PlayerScore = 0;//public player score for reading and possible outside influence
        public int ScorePerMole = 100;//Score per mole succesful mole hit to be set in the editor

        //Get all moles in children
        void InitMoleCollection()
        {
            MoleCollection.Clear();
            MoleCollection.AddRange(GetComponentsInChildren<WAM_MoleScript>());
            foreach (var it in MoleCollection)
            {
                it.myGameManager = this;
            }
        }

        //Rudimentary function to set up a new pattern of moles in the field
        void newPattern()
        {
            //Every mole gets a 50:50 chance to be in the up or down position
            foreach(var it in MoleCollection)
            {
                if (it != null)
                {
                    if (Random.value > 0.5f)
                    {
                        it.MoveMoleUp();
                    }
                    else
                    {
                        it.MoveMoleDown();
                    }
                }
            }
        }

        //Reset function for in the end of the game, sets all moles down
        void resetMoles()
        {
            foreach (var it in MoleCollection)
            {
                if (it != null)
                {
                    it.MoveMoleDown();
                }
            }
        }

        //Called when the game ends
        void StateToEnd()
        {
            if (myState == GameState.InGame)//Check for when it is not called in game
            {
                myState = GameState.EndGame;//Changing the state to go to the score screen
                WAM_ScoreService.SubmitScore(PlayerScore);//We submit our score to for high score
                resetMoles();//clear the field for neetness.
            }
        }

        //Initialise the game
        public void StartGame()
        {
            PlayerScore = 0;//Reset score
            GameStartTime = Time.time;//start the stopwatch/timer
            LastPatternTransitionMoment = 0;//reset the last transition timer
            myState = GameState.InGame;//change our state to note we are now in game.
        }

        //public function to check if we are in-game
        public bool isPlaying()
        {
            return myState == GameState.InGame;
        }

        //public function called by moles to register a valid hit
        public void RegisterScoreHit()
        {
            if (myState == GameState.InGame)//if we are not in game, it is not a valid hit
            {
                PlayerScore += ScorePerMole;//Add mole score to total score
            }
        }

        //public function called by UI to show the time left in the game
        public float GetTimeLeft()
        {
            return GameTimeLength - (Time.time - GameStartTime);
        }

        void Start()
        {
            myState = GameState.idle;
            InitMoleCollection();
        }

        void Update()
        {
            //Handling states
            switch (myState) {
                case GameState.InGame:
                    //When we are in game
                    if (Time.time - GameStartTime >= GameTimeLength)//if the game has been running longer than the suggested game time
                    {
                        StateToEnd();//We end the game
                    }
                    else//if we are still in game
                    {
                        //We change mole-pattern every PatternTransitionTime seconds
                        if (Time.time - LastPatternTransitionMoment > PatternTransitionTime)
                        {
                            newPattern();
                            LastPatternTransitionMoment = Time.time;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}