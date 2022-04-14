using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopItem : MonoBehaviour
{
    public item item;
    public void TryPurchase(){
        if (data.coins >= item.cost){ 
            data.coins -= item.cost;
            PlayerPrefs.SetInt("Coins", data.coins);
            if(! data.collectedItems.ContainsKey(item.itemName)){
                data.collectedItems.Add(item.itemName, 1);
            } else {
                data.collectedItems[item.itemName]++;
            }
            if(item.itemName == "Base Damage"){
                PlayerPrefs.SetInt("Upgrade0Amount", item.GetAmount());
            }
            print("Item name: " +  item.itemName + " Item amount: " + item.GetAmount());
        }
    }
}
