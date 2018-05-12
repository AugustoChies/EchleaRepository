using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ropescript : NetworkBehaviour {
    [SyncVar]
    public Vector3 startingpoint;
    public GameObject col,limit;
    public float dropspeed;
    public float maxdistance;
    bool going;
	// Use this for initialization
	void Start () {
        going = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (going)
        {
            if (Vector3.Distance(this.gameObject.transform.position, startingpoint) < maxdistance)
            {
                this.gameObject.transform.Translate(0, -1 * dropspeed * Time.deltaTime, 0);
            }
            else
            {
                Ropestop();
            }
        }
        this.GetComponent<LineRenderer>().SetPosition(0, startingpoint);
        this.GetComponent<LineRenderer>().SetPosition(1, this.transform.position);
    }
    void Ropestop()
    {
        going = false;
        col.GetComponent<BoxCollider2D>().offset = new Vector2(0, Vector3.Distance(this.gameObject.transform.position, startingpoint) * 2.5f);
        col.GetComponent<BoxCollider2D>().size = new Vector2(1, Vector3.Distance(this.gameObject.transform.position, startingpoint) * 5f);
        limit.transform.position = new Vector3(startingpoint.x, startingpoint.y - 0.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "chao")
        {
            Ropestop();
        }
    }
}
