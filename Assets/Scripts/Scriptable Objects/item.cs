using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class item : ScriptableObject
{
    public string itemName;
    public string description;
    public int cost;
    public Sprite image;
    public int GetAmount(){
        int amount = 0;
        data.collectedItems.TryGetValue(this, out amount);
        return amount;
    }
    public void Load(){
        if(!data.collectedItems.ContainsKey(this)){
            data.collectedItems.Add(this, PlayerPrefs.GetInt(name));
        } else {
            data.collectedItems[this] = PlayerPrefs.GetInt(name);
        }
    }
    public void Save(){
        PlayerPrefs.SetInt(name, GetAmount());
    }
}
