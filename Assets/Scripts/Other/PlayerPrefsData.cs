using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerPrefsData : MonoBehaviour
{
    public static item[] loadedItems;
    void Awake()
    {
        PlayerPrefs.Save();

        if(PlayerPrefs.GetInt("LevelsUnlocked", 1) < 1){
            data.levelsUnlocked = 1;
        } else {
            data.levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked");
        }

        data.coins = PlayerPrefs.GetInt("Coins", 0);
        LoadAllItems();
    }
    public static void LoadAllItems(){
        loadedItems = Resources.LoadAll("Items", typeof(item)).Select(i => (item) i).ToArray();
        foreach(item i in loadedItems){
            i.Load();
        }
    }
}
