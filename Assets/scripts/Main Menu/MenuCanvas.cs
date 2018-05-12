using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : MonoBehaviour {

    public GameObject title, credits, controlsexp, controlsbird;
    public GameObject startb, creditsb, controlsb, exitb, backb;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Start_Button()
    {
        //nada ainda
    }

    public void Credits_Button()
    {
        title.SetActive(false);
        credits.SetActive(true);

        startb.SetActive(false);
        creditsb.SetActive(false);
        controlsb.SetActive(false);
        exitb.SetActive(false);

        backb.SetActive(true);
    }

    public void Controls_Button()
    {
        title.SetActive(false);
        controlsbird.SetActive(true);
        controlsexp.SetActive(true);

        startb.SetActive(false);
        creditsb.SetActive(false);
        controlsb.SetActive(false);
        exitb.SetActive(false);

        backb.SetActive(true);
    }

    public void Exit_Button()
    {
        Application.Quit();
    }

    public void Back_Button()
    {
        title.SetActive(true);
        credits.SetActive(false);
        controlsbird.SetActive(false);
        controlsexp.SetActive(false);

        startb.SetActive(true);
        creditsb.SetActive(true);
        controlsb.SetActive(true);
        exitb.SetActive(true);

        backb.SetActive(false);
    }
}
