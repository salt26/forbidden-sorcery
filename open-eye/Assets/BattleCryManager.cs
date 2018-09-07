using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCryManager : MonoBehaviour {

    public AudioClip soundClick;
    AudioSource myAudio;

    public static BattleCryManager instance;

    void Awake()
    {
        if (BattleCryManager.instance == null)
        {
            BattleCryManager.instance = this;
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
