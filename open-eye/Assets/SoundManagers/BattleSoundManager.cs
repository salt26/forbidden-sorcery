using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSoundManager : MonoBehaviour
{

    public AudioClip soundClick;
    AudioSource myAudio;

    public static BattleSoundManager instance;

    void Awake()
    {
        if (BattleSoundManager.instance == null)
        {
            BattleSoundManager.instance = this;
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
