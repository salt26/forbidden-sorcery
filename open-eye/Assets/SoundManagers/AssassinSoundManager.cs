using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinSoundManager : MonoBehaviour
{

    public AudioClip soundClick;
    AudioSource myAudio;

    public static AssassinSoundManager instance;

    void Awake()
    {
        if (AssassinSoundManager.instance == null)
        {
            AssassinSoundManager.instance = this;
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
