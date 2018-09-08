using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WholeScreen : MonoBehaviour {

    int screenShotNumber = 1;

	public void ScreenBeingClicked()
    {
        Debug.Log("asd");
        if (screenShotNumber == 19)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
        else
        {
            GameObject.Find(screenShotNumber.ToString()).GetComponent<Image>().enabled = false;
            ++screenShotNumber;
            GameObject.Find(screenShotNumber.ToString()).GetComponent<Image>().enabled = true;
        }
        
    }
    
}
