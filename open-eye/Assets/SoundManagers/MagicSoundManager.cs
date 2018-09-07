using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSoundManager : MonoBehaviour
{

    public AudioClip soundClick;
    AudioSource myAudio;

    public static MagicSoundManager instance;

    void Awake()
    {
        if (MagicSoundManager.instance == null)
        {
            MagicSoundManager.instance = this;
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
