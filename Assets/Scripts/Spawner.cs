using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    int counter = 0;
    [SerializeField] bool scaleWithEnemyLordHealth;
    public float timeToSpawn;
    float initialTimeToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        initialTimeToSpawn = timeToSpawn;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (BigEnemyGuy.isDonePart2Intro){
            timeToSpawn = initialTimeToSpawn - (BossScript.MaxHealth - BossScript.Health) / 30;
        }

        counter += 1;
        if(counter >= timeToSpawn)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation); 
            counter = 0;
        }
    }
}
