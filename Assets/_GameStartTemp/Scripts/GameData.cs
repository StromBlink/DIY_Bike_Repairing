using UnityEngine;

namespace Keybored
{

    public class GameData
    {
        
        
        
        
        public static int BestScore
        {
            get { return PlayerPrefs.GetInt("BestScore", 0); }
            set { PlayerPrefs.SetInt("BestScore", value); }
        }

        public static int BestTurn
        {
            get { return PlayerPrefs.GetInt("BestTurn", 0); }
            set { PlayerPrefs.SetInt("BestTurn", value); }
        }




        public static float PlayerExp
        {
            get { return PlayerPrefs.GetFloat("PlayerExp", 0); }
            set { PlayerPrefs.SetFloat("PlayerExp", value); }
        }


        public static float PlayerMaxExp
        {
            get
            {
                return PlayerLevel * 500;
            }
        }


        public static int PlayerLevel
        {
            get { return PlayerPrefs.GetInt("PlayerLevel", 1); }
            set { PlayerPrefs.SetInt("PlayerLevel", value); }
        }

        public static int ReviewCount
        {
            get { return PlayerPrefs.GetInt("ReviewCount", 0); }
            set { PlayerPrefs.SetInt("ReviewCount", value); }
        }

        public static bool ReviewSuccess
        {
            get { return PlayerPrefs2.GetBool("ReviewSuccess"); }
            set { PlayerPrefs2.SetBool("ReviewSuccess", value); }
        }

        public static int Save_Turn
        {
            get { return PlayerPrefs.GetInt("Save_Turn", 0); }
            set { PlayerPrefs.SetInt("Save_Turn", value); }
        }

        public static int Save_Score
        {
            get { return PlayerPrefs.GetInt("Save_Score", 0); }
            set { PlayerPrefs.SetInt("Save_Score", value); }
        }


        public static int Turn
        {
            get { return PlayerPrefs.GetInt("Save_Turn", 0); }
            set { PlayerPrefs.SetInt("Save_Turn", value); }
        }

        public static int Score
        {
            get { return PlayerPrefs.GetInt("Save_Score", 0); }
            set { PlayerPrefs.SetInt("Save_Score", value); }
        }



        public static int Select_Icon
        {
            get { return PlayerPrefs.GetInt("Select_Icon", 0); }
            set { PlayerPrefs.SetInt("Select_Icon", value); }
        }


        public static int CountPlay
        {
            get { return PlayerPrefs.GetInt("CountPlay", 0); }
            set { PlayerPrefs.SetInt("CountPlay", value); }
        }

        public static int CountBreakBricks
        {
            get { return PlayerPrefs.GetInt("CountBreakBricks", 0); }
            set { PlayerPrefs.SetInt("CountBreakBricks", value); }
        }

        public static int CountAllClear
        {
            get { return PlayerPrefs.GetInt("CountAllClear", 0); }
            set { PlayerPrefs.SetInt("CountAllClear", value); }
        }

        public static int CountLuckyBonus
        {
            get { return PlayerPrefs.GetInt("CountLuckyBonus", 0); }
            set { PlayerPrefs.SetInt("CountLuckyBonus", value); }
        }

        public static int CountHighestCombo
        {
            get { return PlayerPrefs.GetInt("CountHighestCombo", 0); }
            set { PlayerPrefs.SetInt("CountHighestCombo", value); }
        }

        public static int BgmCount
        {
            get
            {
                return PlayerPrefs.GetInt("BgmCount", 0);
            }
            set
            {
                PlayerPrefs.SetInt("BgmCount", value);
            }
        }

        public static bool RandomADs
        {
            get { return PlayerPrefs2.GetBool("RandomADs"); }
            set { PlayerPrefs2.SetBool("RandomADs", value); }
        }
        public static bool NoADs
        {
            get { return PlayerPrefs2.GetBool("NOADS_KEY"); }
            set { PlayerPrefs2.SetBool("NOADS_KEY", value); }
        }



        public static string CountryCode
        {
            get { return PlayerPrefs.GetString("CountryCode", "us"); }
            set { PlayerPrefs.SetString("CountryCode", value); }
        }

        public static string NickName
        {
            get { return PlayerPrefs.GetString("NickName", ""); }
            set { PlayerPrefs.SetString("NickName", value); }
        }

        public static bool HapticOff
        {
            get { return PlayerPrefs2.GetBool("HapticOff"); }
            set { PlayerPrefs2.SetBool("HapticOff", value); }
        }




        public static bool AutoLogin
        {
            get { return PlayerPrefs2.GetBool("AutoLogin"); }
            set { PlayerPrefs2.SetBool("AutoLogin", value); }
        }


        public static int Coin
        {
            get { return PlayerPrefs.GetInt("GOLD_SAVE_STRING", 0); }
            set
            {
                PlayerPrefs.SetInt("GOLD_SAVE_STRING", value);
                PlayerPrefs.Save();
            }
        }

        public static int PlayedLevelNumber
        {
            get { return PlayerPrefs.GetInt("PlayedLevelNumber", 0); }
            set
            {
                PlayerPrefs.SetInt("PlayedLevelNumber", value);
            }
        }

        public static int Gem
        {
            get { return PlayerPrefs.GetInt("Gem", 0); }
            set
            {
                PlayerPrefs.SetInt("Gem", value);
            }
        }




        public static int SelectItemNum
        {
            get { return PlayerPrefs.GetInt("SelectBallNum", 0); }
            set { PlayerPrefs.SetInt("SelectBallNum", value); }
        }
        public static int SelectAvatarNum
        {
            get { return PlayerPrefs.GetInt("SelectAvatarNum", 0); }
            set { PlayerPrefs.SetInt("SelectAvatarNum", value); }
        }


        public static int SavePlayerSpec1
        {
            //Speed
            get { return PlayerPrefs.GetInt("SavePlayerSpec1", 400); }
            set { PlayerPrefs.SetInt("SavePlayerSpec1", value); }
        }
        public static int SavePlayerSpec2
        {
            // Fire Range
            get { return PlayerPrefs.GetInt("SavePlayerSpec2", 30); }
            set { PlayerPrefs.SetInt("SavePlayerSpec2", value); }
        }
        // Damage
        public static int SavePlayerSpec3
        {
            get { return PlayerPrefs.GetInt("SavePlayerSpec3", 2); }
            set { PlayerPrefs.SetInt("SavePlayerSpec3", value); }
        }
        public static int SavePlayerSkin1 // Option
        {
            get { return PlayerPrefs.GetInt("SavePlayerSkin1", 0); }
            set { PlayerPrefs.SetInt("SavePlayerSkin1", value); }
        }
    }


}