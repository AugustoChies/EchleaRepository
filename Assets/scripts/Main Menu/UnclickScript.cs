using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnclickScript : NetworkBehaviour {

    GameObject[] pobjects;
    float count = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(pobjects == null)
        {
            pobjects = GameObject.FindGameObjectsWithTag("netplayerobject");
        }

        foreach (GameObject player in pobjects)
        {
            if(player.GetComponent<PlayerObjsScript>().AmILocal())
            {
                player.GetComponent<PlayerObjsScript>().didIClick = false;
            }
        }

        count++;
        if( count > 3)
        {
            Destroy(this.gameObject);
        }
    }
}
