using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (explorer == null)
        {
            explorer = GameObject.Find("Explorer(Clone)");
        }

        if (bird == null)
        {
            bird = GameObject.Find("Bird(Clone)");
        }
        canvas.GetComponent<CanvasScript>().keys = keys;
    }

    public void sendInfo(int what)
    {
        if(explorer != null)
            explorer.GetComponent<Expmove>().UpdadeInventory(this.gameObject,what);
        if (bird != null)
            bird.GetComponent<Birdmove>().UpdadeInventory(this.gameObject,what);
    }
    
}
