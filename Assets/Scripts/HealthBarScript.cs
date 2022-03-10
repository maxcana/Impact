using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarScript : MonoBehaviour
{
    private Image HealthBarImage;
    [SerializeField] enums.Bosses currentBoss;
    private float MaxHealth;
    private float Health;
    BossScript Boss;
    [SerializeField] BigEnemyGuy EnemyBoss;
    private void Start()
    {
        if(currentBoss == enums.Bosses.Hex){
            MaxHealth = BossScript.MaxHealth; } else {
                if(currentBoss == enums.Bosses.BigEnemy){
                    MaxHealth = EnemyBoss.MaxHealth; }

        HealthBarImage = GetComponent<Image>();
        Boss = FindObjectOfType<BossScript>();
        }
    }
    private void Update()
    {
        if(currentBoss == enums.Bosses.Hex){
            MaxHealth = BossScript.MaxHealth; } else {
                if(currentBoss == enums.Bosses.BigEnemy){
                    MaxHealth = EnemyBoss.MaxHealth; }}

        if(currentBoss == enums.Bosses.Hex){
            Health = BossScript.Health; } else {
                if(currentBoss == enums.Bosses.BigEnemy){
                    Health = EnemyBoss.Health; }}

        HealthBarImage.fillAmount += functions.valueMoveTowards(HealthBarImage.fillAmount, Health / MaxHealth, 10);
    }
}
    