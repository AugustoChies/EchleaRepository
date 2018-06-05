using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Inventoryscr : NetworkBehaviour {
    [SyncVar]
    public int keys;
    [SyncVar]
    public bool relic1;
    [SyncVar]
    public bool relic2;
    [SyncVar]
    public bool relic3;
    [SyncVar]
    public bool relic4;
    [SyncVar]
    public bool relic5;
    [SyncVar]
    public bool relic6;
    [SyncVar]
    public bool relic7;
    [SyncVar]
    public bool relic8;
    [SyncVar]
    public bool relic9;

    public GameObject bird;
    public GameObject explorer;
    public GameObject canvas;

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
        }

        if (bird == null)
        {
            bird = GameObject.Find("Bird(Clone)");
        }
        canvas.GetComponent<CanvasScript>().keys = keys;
        canvas.GetComponent<CanvasScript>().relic1 = relic1;
        canvas.GetComponent<CanvasScript>().relic2 = relic2;
        canvas.GetComponent<CanvasScript>().relic3 = relic3;
        canvas.GetComponent<CanvasScript>().relic4 = relic4;
        canvas.GetComponent<CanvasScript>().relic5 = relic5;
        canvas.GetComponent<CanvasScript>().relic6 = relic6;
        canvas.GetComponent<CanvasScript>().relic7 = relic7;
        canvas.GetComponent<CanvasScript>().relic8 = relic8;
        canvas.GetComponent<CanvasScript>().relic9 = relic9;

    }

    public void sendInfo(int what)
    {
        if(explorer != null)
            explorer.GetComponent<Expmove>().UpdadeInventory(this.gameObject,what);
        if (bird != null)
            bird.GetComponent<Birdmove>().UpdadeInventory(this.gameObject,what);
    }
    
}
