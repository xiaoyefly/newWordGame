using System.Collections.Generic;
using UnityEngine;

namespace Main.Logic.Player
{
    public class LGameManager : Main.Utility.Singleton<LGameManager>
    {
        public Dictionary<string, GameConfigData> AllGameDict = new Dictionary<string, GameConfigData>();

        public LGameManager()
        {
            AllGameDict.Add("Wordle",new GameConfigData()
            {
                GameName="Wordle",
                IconPath="UI/Game/Wordle/ICON/Wordle_Icon"
                
            });
        }
        public class GameConfigData
        {
            public string GameName = "";
            public string IconPath = "";
        }
        
    }
}