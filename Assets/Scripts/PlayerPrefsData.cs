using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsData : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.GetInt("LevelsUnlocked") < 1){
            WinZone.levelsUnlocked = 1;
        } else {
            WinZone.levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked");
        }
    }
}
