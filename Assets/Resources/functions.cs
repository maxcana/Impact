using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class functions : MonoBehaviour
{
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

}
