using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stuff : MonoBehaviour
{
    private void Update() {
        if(Input.GetKeyDown("h")){
            SceneManager.LoadScene("Level Select");
        }
    }
}
