using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinescript : MonoBehaviour {
    GameObject exp, bird;
	// Use this for initialization
	void Start () {
        
            exp = GameObject.Find("Explorer(Clone)");

            bird = GameObject.Find("Bird(Clone)");        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Destroy(this.gameObject);
    }
}
