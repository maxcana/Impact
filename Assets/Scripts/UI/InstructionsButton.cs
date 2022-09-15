using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class InstructionsButton : MonoBehaviour
{
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] RectTransform instructionsTitle;
    [SerializeField] RectTransform instructionsStuff;
    [SerializeField] CanvasGroup instructionsStuffGroup;
    [SerializeField] Volume effect;
    public bool isInstructionsOpen;
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        instructionsPanel.SetActive(isInstructionsOpen);
        button.onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        isInstructionsOpen = true;
        instructionsPanel.SetActive(true);
        instructionsStuff.anchoredPosition = new Vector2(0, -130);
        instructionsStuffGroup.alpha = 0;
    }
    private void Update()
    {
        if (isInstructionsOpen)
        {
            instructionsStuff.anchoredPosition += new Vector2(0, functions.valueMoveTowards(instructionsStuff.anchoredPosition.y, -80, 10));
            instructionsStuffGroup.alpha += functions.valueMoveTowards(instructionsStuffGroup.alpha, 1, 15);
            instructionsTitle.localScale += new Vector3(functions.valueMoveTowards(instructionsTitle.localScale.x, 1, 15), 0, 0);
            effect.weight += functions.valueMoveTowards(effect.weight, 1, 10);
        }
        else
        {
            instructionsStuff.anchoredPosition += new Vector2(0, functions.valueMoveTowards(instructionsStuff.anchoredPosition.y, -130, 10));
            instructionsStuffGroup.alpha += functions.valueMoveTowards(instructionsStuffGroup.alpha, 0, 15);
            instructionsTitle.localScale += new Vector3(functions.valueMoveTowards(instructionsTitle.localScale.x, 0, 15), 0, 0);
            effect.weight += functions.valueMoveTowards(effect.weight, 0, 10);
        }
        if(Mathf.Round(effect.weight * 10) / 10 == 0){
            instructionsPanel.SetActive(false);
            effect.weight = 0;
        }
    }
        public void PlayAudio(AudioClip audio){
        GameAssets.i.sound.PlayOneShot(audio);
    }

}
