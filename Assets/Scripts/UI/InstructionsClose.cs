using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsClose : MonoBehaviour
{
    [SerializeField] AudioClip click;
    [SerializeField] InstructionsButton instructions;
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        GameAssets.i.sound.PlayOneShot(click);
        instructions.isInstructionsOpen = false;
    }
}
