using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WholeScreen : MonoBehaviour {

    int screenShotNumber = 0;

	public void ScreenBeingClicked()
    {
        if (screenShotNumber > 0)
        {
            GameObject.Find(screenShotNumber.ToString()).GetComponent<Image>().enabled = false;
        }
        ++screenShotNumber;
        GameObject.Find(screenShotNumber.ToString()).GetComponent<Image>().enabled = true;
    }
    
}
