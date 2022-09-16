using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class ChangeLevelsUnlocked : MonoBehaviour
{
    public Action<int> onUnlockLevel;
    Button button;
    TextMeshProUGUI textMesh;
    int cost;
    private void Awake()
    {
        textMesh = GameObject.FindWithTag("CostToUnlockNextLevel").GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(OnClick);
        textMesh.text = GetCost().ToString();
    }
    public void tryUnlockLevel()
    {
        cost = GetCost();
        if (data.coins >= cost)
        {
            data.coins -= cost;
            PlayerPrefs.SetInt("Coins", data.coins);
            data.levelsUnlocked++;
            PlayerPrefs.SetInt("LevelsUnlocked", data.levelsUnlocked);
            textMesh.text = GetCost().ToString();
            onUnlockLevel? .Invoke(data.levelsUnlocked);
        }
    }
    void OnClick()
    {
        tryUnlockLevel();
    }
    int GetCost()
    {
        return functions.IsBetween(1, 5 + 1, data.levelsUnlocked + 1) ? 25
                    : functions.IsBetween(5, 10, data.levelsUnlocked + 1) ? 100
                        : functions.IsBetween(10, 15 + 1, data.levelsUnlocked + 1) ? 150
                            : functions.IsBetween(15, 20, data.levelsUnlocked + 1) ? 375
                                : functions.IsBetween(20, 25 + 1, data.levelsUnlocked + 1) ? 725
                                    : functions.IsBetween(25, 29 + 1, data.levelsUnlocked + 1) ? 1625
                                        : data.levelsUnlocked + 1 == 10 ? 500
                                            : data.levelsUnlocked + 1 == 20 ? 1000
                                                : data.levelsUnlocked + 1 == 30 ? 3500
                                                    : 69;
    }
}
