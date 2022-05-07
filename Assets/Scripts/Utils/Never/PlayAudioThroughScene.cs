using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioThroughScene : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
    }
    public void PlayAudio()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
        if (GameObject.FindGameObjectsWithTag("WinZone").Length > 1)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
