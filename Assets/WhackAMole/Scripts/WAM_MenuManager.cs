using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OzzysWhackAMole
{
    public class WAM_MenuManager : MonoBehaviour
    {
        public enum WAM_MenuScreen { MainMenu, Game, Score };
        WAM_MenuScreen CurrentMenuScreen;

        public WAM_MenuScreen startScreen = WAM_MenuScreen.MainMenu;

        public WAM_GameManager WAMGame;



        [System.Serializable]
        public class menuSet
        {
            public WAM_MenuScreen MenuScreen;
            public List<GameObject> screenComponents = new List<GameObject>();
            public void TurnOnOff(bool TurnOn)
            {
                if (screenComponents == null) return;
                foreach (var it in screenComponents)
                {
                    if (it != null) it.SetActive(TurnOn);
                }
            }
        }

        public List<menuSet> MenuScreens = new List<menuSet>();

        public void GotoMenu(WAM_MenuScreen goHere)
        {
            if (MenuScreens == null) return;
            var newMenuSet = MenuScreens.Find(X => X.MenuScreen == goHere);
            if (newMenuSet != null)
            {
                foreach (var it in MenuScreens)
                {
                    if (it != null) it.TurnOnOff(false);
                }
                newMenuSet.TurnOnOff(true);
                CurrentMenuScreen = goHere;
            }
        }



        public void StartGame()
        {
            if (WAMGame != null)
            {
                WAMGame.StartGame();
                GotoMenu(WAM_MenuScreen.Game);
            }
        }

        public void GotoMain()
        {
            GotoMenu(WAM_MenuScreen.MainMenu);
        }


        void Start()
        {
            GotoMenu(startScreen);
        }

        void Update()
        {
            switch (CurrentMenuScreen)
            {
                case WAM_MenuScreen.Game:
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