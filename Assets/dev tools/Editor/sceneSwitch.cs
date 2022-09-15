using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;


public class sceneSwitch : Editor
{
    //!! False for build
    
    
    [MenuItem("Scenes/Next Scene")]
    static void NextScene(){
        List<string> scenes = new List<string>();
        foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if(scene.enabled){
                scenes.Add(scene.path);
            }
        }
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()){
            EditorSceneManager.OpenScene(scenes[EditorSceneManager.GetActiveScene().buildIndex + 1]);
        }
    }

    [MenuItem("Scenes/Last Scene")]
    static void LastScene(){
         List<string> scenes = new List<string>();
        foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if(scene.enabled){
                scenes.Add(scene.path);
            }
        }
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()){
            EditorSceneManager.OpenScene(scenes[EditorSceneManager.GetActiveScene().buildIndex - 1]);
        }
    }
    [MenuItem("Scenes/Main Menu")]
    static void MainMenu(){
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()){
            EditorSceneManager.OpenScene("Assets/Scenes/Main Menu.unity");
        }
        
    }
    
}
