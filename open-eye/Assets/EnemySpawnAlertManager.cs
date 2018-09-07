using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnAlertManager : MonoBehaviour
{

    public AudioClip soundClick;
    AudioSource myAudio;

    public static EnemySpawnAlertManager instance;

    void Awake()
    {
        if (EnemySpawnAlertManager.instance == null)
        {
            EnemySpawnAlertManager.instance = this;
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
