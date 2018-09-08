using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButtonScript : MonoBehaviour {

    public GameObject credits;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void MakeCreditsActive()
    {
        credits.SetActive(true);
    }
}
