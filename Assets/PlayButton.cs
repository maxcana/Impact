using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayButton : MonoBehaviour 
{
    [SerializeField] string sceneToLoad;
    float desiredSize = 1;
    float currentSize = 1;
    float bounce;
    Button button;
    private void Awake() {
        button = GetComponent<Button>();
    }
    private void Start() {
        button.onClick.AddListener(OnClick);
    }   
    public void SetDesiredSize(float newDesiredSize){
        desiredSize = newDesiredSize;
    }
    private void FixedUpdate() {
        bounce *= 0.8f;
        bounce += (desiredSize - currentSize) / 3;
    }
    void Update(){
        currentSize += bounce * Time.deltaTime * 16;
        transform.localScale = new Vector2(currentSize, currentSize);
    }
    void OnClick(){
        currentSize -= 0.5f;
        SceneManager.LoadScene(sceneToLoad);
    }
}
