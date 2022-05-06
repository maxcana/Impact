using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    int counter = 0;
    [SerializeField] bool spawnAtRandomVelocity;
    [SerializeField] bool scaleWithEnemyLordHealth;
    public float timeToSpawn;
    float initialTimeToSpawn;
    [SerializeField] BigEnemyGuy beg;

    // Start is called before the first frame update
    void Start()
    {
        initialTimeToSpawn = timeToSpawn;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(scaleWithEnemyLordHealth){
            if (BigEnemyGuy.isDonePart2Intro){
                timeToSpawn = initialTimeToSpawn - (beg.MaxHealth - beg.Health) / 30;
            }
        }

        counter += 1;
        if(counter >= timeToSpawn)
        {
            GameObject myObject = Instantiate(objectToSpawn, transform.position, transform.rotation); 
            if(spawnAtRandomVelocity){
                myObject.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-30, 30), UnityEngine.Random.Range(-30, 30));
            }
            counter = 0;
        }
    }
}
