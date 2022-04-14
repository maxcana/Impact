using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefsData : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.Save();

        if(PlayerPrefs.GetInt("LevelsUnlocked") < 1){
            data.levelsUnlocked = 1;
        } else {
            data.levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked");
        }

        data.coins = PlayerPrefs.GetInt("Coins");
        if(!data.collectedItems.ContainsKey("Base Damage")){
            data.collectedItems.Add("Base Damage", PlayerPrefs.GetInt("Upgrade0Amount"));
        }
         if(PlayerPrefs.GetInt("Upgrade0Amount") < 0){
            if(!data.collectedItems.ContainsKey("Base Damage")){
                data.collectedItems.Add("Base Damage", PlayerPrefs.GetInt("Upgrade0Amount"));
            } else { data.collectedItems["Base Damage"] = PlayerPrefs.GetInt("Upgrade0Amount"); }
        }
    }
}
