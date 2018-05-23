using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseScript : MonoBehaviour {

    public GameObject netmanager, swapcontrol;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (netmanager == null)
        {
            netmanager = GameObject.Find("Network manager");
        }
        if (swapcontrol == null)
        {
            swapcontrol = GameObject.Find("ChangeController");
        }
    }

    public void Custom_Back_Button()
    {
        netmanager.GetComponent<NMOverwriter>().DisconnectCall();
    }

    public void Retry_Button()
    {
        if (swapcontrol.GetComponent<SwapNetwork>().mypobject.GetComponent<PlayerObjsScript>().AmIServer())
            netmanager.GetComponent<NMOverwriter>().ChangeScene("Stage1");
    }
}
