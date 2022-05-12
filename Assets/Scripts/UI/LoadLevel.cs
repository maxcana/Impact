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
    private void Awake()
    {
        button = GetComponent<Button>();
        if (transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != null)
        {
            textValue = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        }
        button.onClick.AddListener(OnClick);
    }
    private void Start()
    {
        GameObject.FindObjectOfType<ChangeLevelsUnlocked>().onUnlockLevel += Refresh;
        Refresh(data.levelsUnlocked);
    }

    void Refresh(int levelsUnlocked)
    {
        if (textValue.Contains("hallenge"))
        {
            //? ridiculous hack, gets last character of words "Challenge (6,5,4,3,2,1)" and checks if your unlocked levels are >= to 5x that
            //? (eg. string = "Challenge 6") are your unlocked levels bigger or equal to 6x5? if yes, it unlocks it, if no it disables it
            if (levelsUnlocked >= int.Parse(textValue.ToCharArray()[textValue.ToCharArray().Length - 1].ToString()) * 5)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }

        }
        else
        {
            print(gameObject.name + " at position " + gameObject.transform.GetSiblingIndex() + "'s textvalue is " + int.Parse(textValue) + " and levels unlocked is " + data.levelsUnlocked);
            if (levelsUnlocked >= int.Parse(textValue))
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }

        }
    }
    void OnClick()
    {
        string x = ("level" + textValue);
        SceneManager.LoadScene("" + x);
        print("Tried to load: " + x);
    }
}
