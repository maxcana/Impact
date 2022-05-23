using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopStatistic : MonoBehaviour
{
    public item item;
    [SerializeField] statistic statToGet;
    public enum statistic { BaseDamage, ExplosionForce, Mass }
    TextMeshProUGUI textMesh;
    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        switch (statToGet)
        {
            case statistic.BaseDamage:
                textMesh.text = "Base Damage: " + data.baseDamage + "\n" + item.GetAmount() + " Items Purchased";
                break;
            case statistic.ExplosionForce:
                textMesh.text = "Explosion Force: " + data.explosionForce + "\n" + item.GetAmount() + " Items Purchased";
                break;
            case statistic.Mass:
                textMesh.text = "Mass: " + data.mass + "\n" + item.GetAmount() + " Items Purchased";
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
