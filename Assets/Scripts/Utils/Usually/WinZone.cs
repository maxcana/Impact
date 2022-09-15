using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    [SerializeField] bool returnToMenu;
    void Start()
    {
        Debug.LogWarning("When adding new scenes, watch out!");
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        if (!returnToMenu)
        {
            string nextLevelName = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
            int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextLevelName.Contains("Level") && !nextLevelName.Contains("Challenge"))
            {
                GameObject.FindWithTag("WinZoneSound").GetComponent<PlayAudioThroughScene>().PlayAudio();
                PlayerPrefs.Save();
                SceneManager.LoadScene(nextLevel);
                if (nextLevel > data.levelsUnlocked)
                {
                    
                    //! -4 for the main menu and level select, shop, credits
                    data.levelsUnlocked = nextLevel - 4;
                    PlayerPrefs.SetInt("LevelsUnlocked", data.levelsUnlocked);
                }
            }
            else
            {
                PlayerPrefs.Save();
                SceneManager.LoadScene("Level Select");
            }
            
        }
        else
        {
            PlayerPrefs.Save();
            SceneManager.LoadScene("Level Select");
        }
        
    }
}
