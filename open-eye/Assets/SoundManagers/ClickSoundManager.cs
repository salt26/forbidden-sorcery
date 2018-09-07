using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSoundManager : MonoBehaviour
{
    public AudioClip soundClick; 
    AudioSource myAudio;

    public static ClickSoundManager instance; 

    void Awake()
    {
        if (ClickSoundManager.instance == null) 
        {
            ClickSoundManager.instance = this;
        }
    }
    void Start()
    {
        myAudio = this.gameObject.GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        myAudio.PlayOneShot(soundClick);
    }
}
