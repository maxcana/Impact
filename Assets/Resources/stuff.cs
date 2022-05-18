using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class stuff : MonoBehaviour
{
    [SerializeField] item damageItem;
    [SerializeField] item explosionForceItem;
    private void Awake()
    {
        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        string[] gameObjectTags = new string[gameObjects.Length];
        int i = 0;
        foreach (var gameObject in gameObjects)
        {
            gameObjectTags[i] = gameObject.tag;
            i++;
        }
        if (!gameObjectTags.Contains("WinZoneSound"))
        {
            Instantiate(Resources.Load("WinZoneSound") as GameObject);
        }
    }
    private void Start()
    {
        print("stuff says hi");
    }

    private void Update()
    {
        data.baseDamage = 10 + damageItem.GetAmount() * 5;
        data.explosionForce = 50 + explosionForceItem.GetAmount() * 10;

        //!STUFF IS REQUIRED IN THE BUILD
        if (!SceneManager.GetActiveScene().name.Contains("Level"))
        {
            Time.timeScale = 1;
        }

        //*dev tools
        if (Input.GetKeyDown(KeyCode.R))
        {
            Destroy(GameObject.FindWithTag("Player"));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        if (Input.GetKeyDown("j"))
        {
            //just

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 100);
            PlayerPrefs.SetInt("LevelsUnlocked", 16);
            data.levelsUnlocked = 16;
            data.coins += 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            data.coins = data.coins == 0 ? 1 : data.coins;
            data.coins *= 2;
            PlayerPrefs.SetInt("Coins", data.coins);
        }


        if (Input.GetKeyDown("k"))
        {
            //kidding

            print("wiped save");
            PlayerPrefs.DeleteAll();
            data.collectedItems.Clear();
            data.coins = 0;
            data.levelsUnlocked = 1;
            PlayerPrefs.SetInt("LevelsUnlocked", 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(Input.GetKey("t")){
            // teleport
            GameObject player = GameObject.FindWithTag("Player");
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            player.GetComponent<Rigidbody2D>().velocity = 50 * (new Vector3(pos.x, pos.y, player.transform.position.z) - player.transform.position);
            player.transform.position = pos;
        }
    }
}
