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
        data.collectedItems.TryGetValue(this.itemName, out amount);
        return amount;
    }
}
