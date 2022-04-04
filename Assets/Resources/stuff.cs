using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stuff : MonoBehaviour
{
    [SerializeField] KeyCode[] EraseLevelData = new KeyCode[14];
    [SerializeField] KeyCode[] NextLevel = new KeyCode[9];
    int eraseleveldataindex;
    int nextlevelindex;
    private void Start() {
        eraseleveldataindex = 0;
        nextlevelindex = 0;
    }
    
    private void Update() {

        if(Input.GetKeyDown(EraseLevelData[eraseleveldataindex])){ 
          eraseleveldataindex += 1;
          if(eraseleveldataindex == EraseLevelData.Length){
              data.levelsUnlocked = 1; 
              PlayerPrefs.SetInt("LevelsUnlocked", 1);
           eraseleveldataindex = 0;
          }
        }

        if(Input.GetKeyDown(NextLevel[nextlevelindex])){ 
         nextlevelindex += 1;
         if(nextlevelindex == NextLevel.Length){
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
           nextlevelindex = 0;
         }
        }

        if(Input.GetKeyDown("h")){
            SceneManager.LoadScene("Level Select");
        }
        if(Input.GetKeyDown("j")){
            data.levelsUnlocked = 16;
        }
        if(Input.GetKeyDown("d")){
            print("levelsunlocked: " + data.levelsUnlocked);
            print("playerprefslevelsunlocked: " + PlayerPrefs.GetInt("LevelsUnlocked"));
        }
        if(! SceneManager.GetActiveScene().name.Contains("Level")){
            Time.timeScale = 1;
        }
    }
}
