using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour {

    [SerializeField]
    Sprite victoryImage;

    [SerializeField]
    Sprite loseImage;

    public void ShowVictoryOrLose(bool win)
    {
        GetComponent<Image>().enabled = true;
        if (win)
        {
            GetComponent<Image>().sprite = victoryImage;
        }

        else
        {
            GetComponent<Image>().sprite = loseImage;
        }
    }
}
