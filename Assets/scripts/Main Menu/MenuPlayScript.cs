using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayScript : MonoBehaviour {
    public GameObject bird1, bird2, exp1, exp2;
    public GameObject confirmscreen, waitscreen;
    public GameObject accept, deny;

    public GameObject netmanager,swapcontrol;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(netmanager == null)
        {
            netmanager = GameObject.Find("Network manager");
        }
        if (swapcontrol == null)
        {
            swapcontrol = GameObject.Find("ChangeController");
        }
    }

    public void Back_Button()
    {
       // netmanager.GetComponent<NMOverwriter>().DisconnectCall();
        Destroy(netmanager);
        SceneManager.LoadScene("Main Menu");
    }

    public void Custom_Back_Button()
    {
        netmanager.GetComponent<NMOverwriter>().DisconnectCall();            
    }

    public void Swap_Button()
    {
        swapcontrol.GetComponent<SwapNetwork>().UpdateClicked(true);
    }

    public void ShowWait()
    {
        waitscreen.SetActive(true);
    }

    public void ShowReq()
    {
        waitscreen.SetActive(false);
        confirmscreen.SetActive(true);
        accept.SetActive(true);
        deny.SetActive(true);
    }

    public void Play_Button()
    {
        if(swapcontrol.GetComponent<SwapNetwork>().mypobject.GetComponent<PlayerObjsScript>().AmIServer())
            netmanager.GetComponent<NMOverwriter>().ChangeScene("Stage1");
    }

    void Swap_Images(bool yes)
    {
        if (yes)
        {
            netmanager.GetComponent<NMOverwriter>().SwapChars();
            bird1.SetActive(!bird1.activeSelf);
            bird2.SetActive(!bird2.activeSelf);
            exp1.SetActive(!exp1.activeSelf);
            exp2.SetActive(!exp2.activeSelf);
        }
        
            waitscreen.SetActive(false);
            confirmscreen.SetActive(false);
            accept.SetActive(false);
            deny.SetActive(false);
        
    }

}


