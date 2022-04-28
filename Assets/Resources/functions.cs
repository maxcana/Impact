using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class functions : MonoBehaviour
{
    //!
    public static Vector2 positionMoveTowards(Vector2 currentpos, Vector2 endpos, float speed){
        //? gets the position to move towards by from the start to end position
        return speed * Time.deltaTime * (endpos - currentpos);
    }

    public static float valueMoveTowards(float currentvalue, float endvalue, float speed){
        //? gets the value to move towards by from the start to end value
        return speed * Time.deltaTime * (endvalue - currentvalue);
    }

    public static bool IsBetween(int a, int b, int number)
    {
        return number > a & number < b;
    }
    public static bool IsBetweenf(float a, float b, float number)
    {
        return number > a & number < b;
    }

    public static float Sin(float value, float speed, float multiplier){
        return Mathf.Sin(value * speed) * multiplier;
    }
    public static void SpawnCoins(Vector2 position, float rarity, int amount, float range = 5){
        for(int i = 0; i < amount; i++){
            GameObject coin = Instantiate(Resources.Load("DroppedCoin", typeof(GameObject)), position, Quaternion.identity) as GameObject;
            SpriteRenderer sr = coin.transform.GetChild(0).GetComponent<SpriteRenderer>();
            coin.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(range *- 1, range), UnityEngine.Random.Range(range *- 0.1f, range * 0.8f));
            int worth = 1;

            //! I shall assume that this is the same as using stacked if statements
            worth = TakeChance(Mathf.Clamp(rarity * 5, 0, 100)) ? 10 : TakeChance(Mathf.Clamp(rarity * 10, 0, 100)) ? 5 : 1;
            switch(worth){
                case 1: 
                //!! BUG:why is this red
                    sr.color = new Color(1f, 147 / 255, 0, 1f);
                    break;
                case 5:
                    sr.color = new Color(0, 234 / 255, 1f, 1f);
                    break;
                case 10:
                    sr.color = new Color(1f, 0, 1f, 1f);
                    break;
                default:
                    sr.color = Color.black;
                    Debug.LogError("Coin was worth invalid amount");
                    break;
            }

            coin.transform.GetChild(0).GetComponent<Coin>().worth = worth;
        }
    }
    public static bool TakeChance(float chance){
        return UnityEngine.Random.Range(0f, 100f) <= chance;
    }
}
