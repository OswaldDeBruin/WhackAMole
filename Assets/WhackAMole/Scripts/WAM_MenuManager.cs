﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OzzysWhackAMole
{
    //Main menu made out of a simple state machine
    public class WAM_MenuManager : MonoBehaviour
    {
        public enum WAM_MenuScreen { MainMenu, Game, Score };//Machine states which correspond to menu screens
        public WAM_MenuScreen startScreen = WAM_MenuScreen.MainMenu;//The first screen we want to see on startup
        WAM_MenuScreen CurrentMenuScreen;//The current machine state or menu screen

        public WAM_GameManager WAMGame;//The game manager the menu manages

        //Class used for menu states
        [System.Serializable]
        public class menuSet
        {
            public WAM_MenuScreen MenuScreen;//state we want to attach these elements to
            public List<GameObject> screenComponents = new List<GameObject>();//The screen components we want to see on a menu screen
            //Managing function that handles turning on or off for us
            public void TurnOnOff(bool TurnOn)
            {
                if (screenComponents == null) return;
                foreach (var it in screenComponents)
                {
                    if (it != null) it.SetActive(TurnOn);
                }
            }
        }
        public List<menuSet> MenuScreens = new List<menuSet>();//All menu screen states we want to be able to navigate to

        //This function handles navigation from and to a menu state
        public void GotoMenu(WAM_MenuScreen goHere)
        {
            if (MenuScreens == null) return;
            var newMenuSet = MenuScreens.Find(X => X.MenuScreen == goHere);//Find the menu state to got to
            if (newMenuSet != null)//only navigate to it if it exists
            {
                foreach (var it in MenuScreens)//Turn everything off first
                {
                    if (it != null) it.TurnOnOff(false);
                }
                newMenuSet.TurnOnOff(true);//Turn the navigatable menu screen elements on
                CurrentMenuScreen = goHere;//Remember we are now in this state
            }
        }

        //void function to let button navigate to main menu
        public void GotoMain()
        {
            GotoMenu(WAM_MenuScreen.MainMenu);
        }

        //void function to let button start the game
        public void StartGame()
        {
            if (WAMGame != null)
            {
                WAMGame.StartGame();
                GotoMenu(WAM_MenuScreen.Game);
            }
        }

        void Start()
        {
            GotoMenu(startScreen);//Menu initialisation
        }

        void Update()
        {
            switch (CurrentMenuScreen)//Handling states
            {
                case WAM_MenuScreen.Game:
                    //If we've started the game and the game is not playing anymore (or never existed), we go to the score screen
                    if (WAMGame == null || !WAMGame.isPlaying())
                    {
                        GotoMenu(WAM_MenuScreen.Score);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}