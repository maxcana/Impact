using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stuff : MonoBehaviour
{
    [SerializeField] item damageItem;
    private void Start() {
        print("stuff says hi");
    }
    
    private void Update() {
        data.baseDamage = 10 + damageItem.GetAmount() * 5;

        if(Input.GetKeyDown("j")){
            //just
        
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 100);
            PlayerPrefs.SetInt("LevelsUnlocked", 16);
            data.levelsUnlocked = 16;
            data.coins += 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(Input.GetKeyDown("k")){
            //kidding

            print("wiped save");
            PlayerPrefs.SetInt("LevelsUnlocked", 0);
            PlayerPrefs.SetInt("Coins", 0);
            PlayerPrefs.SetInt("Upgrade0Amount", 0);
            data.collectedItems.Clear();
            data.coins = 0;
            data.levelsUnlocked = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //! TIME SCALE 1 (THIS MAKES STUFF REQUIRED IN THE BUILD)
        if(! SceneManager.GetActiveScene().name.Contains("Level")){
            Time.timeScale = 1;
        }
    }
}
