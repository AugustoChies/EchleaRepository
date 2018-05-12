using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump_detector : MonoBehaviour {
    GameObject player;
	// Use this for initialization
	void Start () {
        player = this.gameObject.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "chao")
        {
            player.GetComponent<Expmove>().canjump = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "chao")
        {
            player.GetComponent<Expmove>().canjump = false;
        }
    }
}
