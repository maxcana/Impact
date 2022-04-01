using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    public static int levelsUnlocked = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Player") return;
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("Tried to load: "+ nextLevel);
        SceneManager.LoadScene(nextLevel);
        if(nextLevel > levelsUnlocked){
            levelsUnlocked = nextLevel;
        }
    }
}
