using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CanvasScript : NetworkBehaviour {
    public GameObject cursetxt, keystxt,wintxt,losetxt;
    [SyncVar]
    public int keys;
    [SyncVar]
    public float curserate;
    public float curse;
    bool spawned;

    [SyncVar]
    public bool won;
    [SyncVar]
    public bool lost;

    public GameObject curseobj;

    public GameObject explorer, bird;
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
            explorer = GameObject.Find("Explorer(Clone)");
        }

        if (bird == null)
        {
            bird = GameObject.Find("Bird(Clone)");
        }
        curse += curserate * Time.deltaTime;

        cursetxt.GetComponent<Text>().text = "" + (int)curse;
        keystxt.GetComponent<Text>().text = "" + keys;

        if (lost && !won)
        {
            losetxt.GetComponent<Text>().enabled = true;
        }
        else if (won)
        {
            wintxt.GetComponent<Text>().enabled = true;
        }

        if(curse > 150 && !spawned)
        {
            spawned = true;
            if (explorer != null)
                explorer.GetComponent<Expmove>().CallCurse(this.gameObject);
            if (bird != null)
                bird.GetComponent<Birdmove>().CallCurse(this.gameObject);
        }
    }
}
