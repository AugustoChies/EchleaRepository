using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayScript : MonoBehaviour {
    public GameObject bird1, bird2, exp1, exp2;
    public GameObject confirmscreen, waitscreen;
    public GameObject clientwait;
    public GameObject accept, deny;

    public GameObject netmanager,swapcontrol;

    public GameObject cerror, herror;

    public bool char1, char2; // true explorador, false passaro

    string typedip;
    float errortimer;

    // Use this for initialization
    void Start()
    {
        typedip = "localhost";
        errortimer = 0;
        char1 = true;
        char2 = false;        
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

        if(errortimer > 0)
        {
            errortimer -= 1 * Time.deltaTime;
            if(errortimer <= 0)
            {
                cerror.SetActive(false);
                herror.SetActive(false);
            }
        }

        if (swapcontrol.GetComponent<SwapNetwork>().switchchar)
        {
            Swap_Images(true);
            swapcontrol.GetComponent<SwapNetwork>().switchchar = false;
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

    public void Custom_Host_Button()
    {
        netmanager.GetComponent<NMOverwriter>().CustomStartHost();
    }

    public void Custom_Client_Button()
    {
        clientwait.SetActive(true);
        netmanager.GetComponent<NMOverwriter>().CustomStartClient(typedip);
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

    public void HideAll()
    {
        waitscreen.SetActive(false);
        confirmscreen.SetActive(false);
        accept.SetActive(false);
        deny.SetActive(false);
    }

    public void Play_Button()
    {
        if (swapcontrol.GetComponent<SwapNetwork>().mypobject.GetComponent<PlayerObjsScript>().AmIServer())
        {
            if (netmanager.GetComponent<NMOverwriter>().numPlayers == 2)
            {
                netmanager.GetComponent<NMOverwriter>().SwapChars(char1, char2);
                netmanager.GetComponent<NMOverwriter>().ChangeScene("Stage1");
            }
            else
            {
                errortimer = 4;
                herror.SetActive(true);
            }
        }
        else
        {
            errortimer = 4;
            cerror.SetActive(true);
        }
    }

    public void Accept_Button()
    {
        swapcontrol.GetComponent<SwapNetwork>().UpdateClicked(false);
        swapcontrol.GetComponent<SwapNetwork>().UpdateCharClicked(false);
        swapcontrol.GetComponent<SwapNetwork>().Swap();
    }

    public void Deny_Button()
    {
        swapcontrol.GetComponent<SwapNetwork>().UpdateClicked(false);
        swapcontrol.GetComponent<SwapNetwork>().UpdateCharClicked(false);
    }

    public void Swap_Images(bool yes)
    {
        if (yes)
        {
            char1 = !char1;
            char2 = !char2;
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

    public void ApplyNewIP(string newv)
    {
        Debug.Log(newv);
        typedip = newv;
    }

}


