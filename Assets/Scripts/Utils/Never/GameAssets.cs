using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    public static GameAssets i {
        get {
            if (_i == null) {
                _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
                _i.Shake = GameObject.FindObjectOfType<CameraShake>();
                _i.sound = _i.GetComponent<AudioSource>();
            } 
            
            return _i;
        }
    }

    public AudioSource sound;
    public Transform DamagePopup;
    public Transform DeathCircle;
    public Transform circleIndicator;
    public Transform DamageParticle;
    public CameraShake Shake;
}
