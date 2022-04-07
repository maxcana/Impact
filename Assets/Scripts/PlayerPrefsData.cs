using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsData : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.GetInt("LevelsUnlocked") < 1){
            data.levelsUnlocked = 1;
        } else {
            data.levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked");
        }

        if(PlayerPrefs.GetInt("Coins") < 0){
            data.coins = 0;
        } else {
            data.coins = PlayerPrefs.GetInt("Coins");
        }
    }
}
