using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class shopItem : MonoBehaviour
{
    public item item;
    public AudioClip purchaseNoise;
    public AudioClip notEnoughCoinsNoise;
    public void TryPurchase(){
        if (data.coins >= item.cost){ 
            data.coins -= item.cost;
            GameAssets.i.sound.PlayOneShot(purchaseNoise);
            PlayerPrefs.SetInt("Coins", data.coins);
            if(! data.collectedItems.ContainsKey(item)){
                data.collectedItems.Add(item, 1);
            } else {
                data.collectedItems[item]++;
            }
            item.Save();
            print("Item name: " +  item.itemName + " Item amount: " + item.GetAmount());
        } else {
            GameAssets.i.sound.PlayOneShot(notEnoughCoinsNoise);
        }
    }
    //hi
    private void Start() {
        TextMeshProUGUI titleText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Image image = transform.GetChild(2).GetComponent<Image>();
        transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = item.cost.ToString();
        image.sprite = item.image;
        description.text = item.description;
        titleText.text = item.itemName;
    }
}
