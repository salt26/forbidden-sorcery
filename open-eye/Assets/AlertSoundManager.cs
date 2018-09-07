using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSoundManager : MonoBehaviour
{

    public AudioClip soundClick;
    AudioSource myAudio;

    public static AlertSoundManager instance;

    void Awake()
    {
        if (AlertSoundManager.instance == null)
        {
            AlertSoundManager.instance = this;
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
