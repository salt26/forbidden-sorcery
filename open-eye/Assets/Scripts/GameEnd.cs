using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour {

    [SerializeField]
    Sprite victoryImage;

    [SerializeField]
    Sprite loseImage;

    [SerializeField]
    GameObject background;

    [SerializeField]
    Sprite victoryBackgroundImage;

    [SerializeField]
    Sprite loseBackgroundImage;

    public void ShowVictoryOrLose(bool win)
    {
        GetComponent<Image>().enabled = true;
        if (win)
        {
            GetComponent<Image>().sprite = victoryImage;
            background.SetActive(true);
            background.GetComponent<Image>().sprite = victoryBackgroundImage;
        }

        else
        {
            GetComponent<Image>().sprite = loseImage;
            background.SetActive(false);
            background.GetComponent<Image>().sprite = loseBackgroundImage;
        }
    }
}
