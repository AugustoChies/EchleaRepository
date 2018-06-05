using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasScript : NetworkBehaviour {
    public GameObject cursetxt, keystxt;
    [SyncVar]
    public int keys;
    public bool relic1, relic2, relic3, relic4, relic5, relic6, relic7, relic8, relic9;
    [SyncVar]
    public float curserate;
    public float curse;
    bool spawned;

    public GameObject pausescreen, disconnectbutton;
    public GameObject irelic1, irelic2, irelic3, irelic4, irelic5, irelic6, irelic7, irelic8, irelic9;

    public GameObject curseobj;

    public GameObject explorer, bird;

    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        keys = 0;
        curserate = 0.1f;
        curse = 0;
        spawned = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (explorer == null)
        {
            if (!(SceneManager.GetActiveScene().name == "Stage1" || SceneManager.GetActiveScene().name == "Stage2" || SceneManager.GetActiveScene().name == "Stage3"))
            {
                Destroy(this.gameObject);
            }
            explorer = GameObject.Find("Explorer(Clone)");
            keys = 0;
            curserate = 0.1f;
            curse = 0;
            spawned = false;
        }

        if (bird == null)
        {
            bird = GameObject.Find("Bird(Clone)");
        }
        curse += curserate * Time.deltaTime;

        cursetxt.GetComponent<Text>().text = "" + (int)curse;
        keystxt.GetComponent<Text>().text = "" + keys;

        if(curse > 150 && !spawned)
        {
            spawned = true;
            if (explorer != null)
                explorer.GetComponent<Expmove>().CallCurse(this.gameObject);
            if (bird != null)
                bird.GetComponent<Birdmove>().CallCurse(this.gameObject);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pausescreen.activeSelf)
            {
                pausescreen.SetActive(false);
                disconnectbutton.SetActive(false);
                irelic1.SetActive(false);
                irelic2.SetActive(false);
                irelic3.SetActive(false);
                irelic4.SetActive(false);
                irelic5.SetActive(false);
                irelic6.SetActive(false);
                irelic7.SetActive(false);
                irelic8.SetActive(false);
                irelic9.SetActive(false);
            }
            else
            {
                pausescreen.SetActive(true);
                disconnectbutton.SetActive(true);
                if(relic1)
                    irelic1.SetActive(true);
                if (relic2)
                    irelic2.SetActive(true);
                if (relic3)
                    irelic3.SetActive(true);
                if (relic4)
                    irelic4.SetActive(true);
                if (relic5)
                    irelic5.SetActive(true);
                if (relic6)
                    irelic6.SetActive(true);
                if (relic7)
                    irelic7.SetActive(true);
                if (relic8)
                    irelic8.SetActive(true);
                if (relic9)
                    irelic9.SetActive(true);
            }
        }
    }

    public void Custom_Back_Button()
    {
        GameObject netmanager = GameObject.Find("Network manager");
        netmanager.GetComponent<NMOverwriter>().DisconnectCall();
    }
}
