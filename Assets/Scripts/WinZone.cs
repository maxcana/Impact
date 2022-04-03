using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    [SerializeField] bool returnToMenu;
    public static int levelsUnlocked;
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Player") return;
        if(!returnToMenu){
                string nextLevelName = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
                int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
                if(nextLevelName.Contains("Level") && !nextLevelName.Contains("Challenge")){
                    SceneManager.LoadScene(nextLevel);
                    if(nextLevel > levelsUnlocked){
                        //! -2 for the main menu and level select (what about tutorial?) well buid settings start at 0 and level names start at 1
                      levelsUnlocked = nextLevel - 2;
                      PlayerPrefs.SetInt("LevelsUnlocked", levelsUnlocked);
                    }
                } else {
                    SceneManager.LoadScene("Level Select");
                }
                
        } else {
            SceneManager.LoadScene("Level Select");
        }
    }
}
