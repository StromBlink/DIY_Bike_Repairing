using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Keybored
{

    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;
        public delegate void GameAction();
        public delegate void GameActionInt(int number);
        public delegate void GameAction2Int(int number1,int number2);

        public delegate void GameActionBool(bool isAction);
        public delegate void GameActionString(string word);

        public event GameActionBool SounChanged;
        public event GameAction levelStart;
        public event GameAction levelLose;
        public event GameAction2Int levelEnd;
        public event GameAction playerRevive;
        public event GameAction playerReward;
        public event GameAction playerWatchRewarded;
        public event GameAction playerSpecChanged;
        public event GameAction playerSkinChanged; // Option
        public event GameAction gameMenu; // Option
        public event GameActionInt playerShowSkin;
        public event GameActionBool inGameMenu;



        private void Awake()
        {
            Instance = this;
        }

        public void LevelStart()
        {
            if (levelStart == null)
                return;
            levelStart.Invoke();
        }

        public void LevelLoss()
        {
            if (levelLose == null)
                return;
            levelLose.Invoke();
        }
        public void GameMenu()
        {
            if (gameMenu == null)
                return;
            gameMenu.Invoke();
        }

        public void PlayerRevive()
        {
            if (playerRevive == null)
                return;
            playerRevive.Invoke();
        }
        public void PlayerReward()
        {
            if (playerReward == null)
                return;
            playerReward.Invoke();
        }
        public void PlayerWatchRewarded()
        {
            if (playerWatchRewarded == null)
                return;
            playerWatchRewarded.Invoke();
        }
        public void PlayerSpecChanged()
        {
            if (playerSpecChanged == null)
                return;
            playerSpecChanged.Invoke();
        }
        public void PlayerSkinChanged()
        {
            if (playerSkinChanged == null)
                return;
            playerSkinChanged.Invoke();
        }
        public void GameMenuChanged(bool open)
        {
            if (inGameMenu == null)
                return;
            inGameMenu.Invoke(open);
        }
        public void PlayerSkinShown(int number)
        {
            if (playerShowSkin == null)
                return;
            playerShowSkin.Invoke(number);
        }
        public void LevelEnd(int score,int gold)
        {
            if (levelEnd == null)
                return;
            levelEnd.Invoke(score,gold);
        }

        public void SoundAction(bool isAction)
        {
            if (SounChanged == null)
                return;
            SounChanged.Invoke(isAction);

        }
    }
}