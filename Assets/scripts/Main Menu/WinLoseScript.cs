using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoseScript : MonoBehaviour {

    public GameObject netmanager, swapcontrol,inventory;
    public GameObject relic1, relic2, relic3, relic4, relic5, relic6, relic7, relic8, relic9;
    bool iswin;
    // Use this for initialization
    void Start () {
        if(SceneManager.GetActiveScene().name == "WinScreen")
        {
            iswin = true;
            inventory = GameObject.Find("InventoryInfo");
        }
		
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

        if(iswin)
        {
            if(inventory.GetComponent<Inventoryscr>().relic1)
            {
                relic1.SetActive(true);
            }
            if (inventory.GetComponent<Inventoryscr>().relic2)
            {
                relic2.SetActive(true);
            }
            if (inventory.GetComponent<Inventoryscr>().relic3)
            {
                relic3.SetActive(true);
            }
            if (inventory.GetComponent<Inventoryscr>().relic4)
            {
                relic4.SetActive(true);
            }
            if (inventory.GetComponent<Inventoryscr>().relic5)
            {
                relic5.SetActive(true);
            }
            if (inventory.GetComponent<Inventoryscr>().relic6)
            {
                relic6.SetActive(true);
            }
            if (inventory.GetComponent<Inventoryscr>().relic7)
            {
                relic7.SetActive(true);
            }
            if (inventory.GetComponent<Inventoryscr>().relic8)
            {
                relic8.SetActive(true);
            }
            if (inventory.GetComponent<Inventoryscr>().relic9)
            {
                relic9.SetActive(true);
            }
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
