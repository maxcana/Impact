using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadLevel : MonoBehaviour
{
    Button button;
    string textValue;
    private void Awake() {
        textValue = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
    private void Start() {
        button = GetComponent<Button>();

        
        if(textValue.Contains("hallenge")){

            //? ridiculous hack, gets last character of words "Challenge (6,5,4,3,2,1)" and checks if your unlocked levels are >= to 5x that
            //? (eg. string = "Challenge 6") are your unlocked levels bigger or equal to 6x5? if yes, it unlocks it, if no it disables it
            if(WinZone.levelsUnlocked >= int.Parse(textValue.ToCharArray()[textValue.ToCharArray().Length - 1].ToString()) * 5){
                button.interactable = true;
                button.onClick.AddListener(OnClick);
            } else {
                button.interactable = false;
            }

        } else {

            if(WinZone.levelsUnlocked >= int.Parse(textValue)){
                button.interactable = true;
                button.onClick.AddListener(OnClick);
            } else {
                button.interactable = false;
            }

        }
        
    }

    void OnClick(){
        string x = ("level" + textValue);
        SceneManager.LoadScene("" + x);
        print("Tried to load: " + x);
    }
}
