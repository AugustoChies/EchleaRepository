using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class spinescript : NetworkBehaviour {
    //GameObject exp, bird;
    [SyncVar]
    public bool direction;
	// Use this for initialization
	void Start () {
        
           
    }
	
	// Update is called once per frame
	void Update () {
		if(direction)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Destroy(this.gameObject);
    }
}
