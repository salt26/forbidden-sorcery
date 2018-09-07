using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayLogic : MonoBehaviour {
    
	void Awake()
    {
        for ( int i = 1 ; i <= 17 ; ++i )
        {
            GameObject.Find(i.ToString()).GetComponent<Image>().enabled = false;
        }
    }
}
