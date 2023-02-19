using UnityEngine;

namespace Main.Logic.Player
{
    public class LPlayer : Main.Utility.Singleton<LPlayer>
    {
        public string Address => PlayerPrefs.HasKey("Account") ? PlayerPrefs.GetString("Account") : "";
    }
}