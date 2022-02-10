using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int counter = 0;
    public int timeToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += 1;
        if(counter >= timeToSpawn)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation); 
            counter = 0;
        }
    }
}
