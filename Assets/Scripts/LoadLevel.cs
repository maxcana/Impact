using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadLevel : MonoBehaviour
{
    Button button;
    private void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick(){
        int x = int.Parse(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        
        //! This is for the tutorial level
        x += 1;
        SceneManager.LoadScene(x);
    }
}
