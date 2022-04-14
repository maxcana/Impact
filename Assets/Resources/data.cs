using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data : MonoBehaviour
{
    public static int levelsUnlocked;
    public static int coins;
    public static float baseDamage;
    public static Dictionary<item, int> collectedItems = new Dictionary<item, int>();
}
