using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadButton : MonoBehaviour 
{
    [SerializeField] string sceneToLoad;
    Button button;
    private void Awake() {
        button = GetComponent<Button>();
    }
    private void Start() {
        button.onClick.AddListener(OnClick);
    }   
    void OnClick(){
        SceneManager.LoadScene(sceneToLoad);
    }
}
