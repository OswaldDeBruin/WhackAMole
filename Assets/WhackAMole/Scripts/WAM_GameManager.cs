using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OzzysWhackAMole
{

    public class WAM_GameManager : MonoBehaviour
    {
        enum GameState { idle, InGame, Score };
        GameState myState = GameState.idle;

        public List<WAM_MoleScript> MoleCollection = new List<WAM_MoleScript>();

        public float GameTimeLength = 30;
        float GameStartTime = 0;

        public float PatternTransitionTime = 3;
        float LastTransitionMoment = 0;


        void InitMoleCollection()
        {
            MoleCollection.Clear();
            MoleCollection.AddRange(GetComponentsInChildren<WAM_MoleScript>());
        }



        void Start()
        {
            InitMoleCollection();
        }

        void newPattern()
        {
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

        void StateToScore()
        {
            myState = GameState.Score;
            resetMoles();
        }

        public void StartGame()
        {
            GameStartTime = Time.time;
            LastTransitionMoment = 0;
            myState = GameState.InGame;
        }

        public bool isPlaying()
        {
            return myState == GameState.InGame;
        }

        // Update is called once per frame
        void Update()
        {
            switch (myState) {
                case GameState.InGame:
                    if (Time.time - GameStartTime >= GameTimeLength)
                    {
                        StateToScore();
                    }
                    else
                    {
                        if (Time.time - LastTransitionMoment > PatternTransitionTime)
                        {
                            newPattern();
                            LastTransitionMoment = Time.time;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}