using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class HowToPlayButtonScript : MonoBehaviour
{

    public void StartHowToPlay()
    {
        SceneManager.LoadScene("HowToPlayScene");
    }
}