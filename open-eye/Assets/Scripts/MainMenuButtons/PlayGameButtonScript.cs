using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayGameButtonScript : MonoBehaviour {

    public void StartMainGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }
}