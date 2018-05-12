using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iconfader : MonoBehaviour {

    float timer;
	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(0, 0.5f * Time.deltaTime, 0);
        this.gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.33f * Time.deltaTime);
        if(timer > 3)
        {
            Destroy(this.gameObject);
        }


        timer += 1 * Time.deltaTime;
	}
}
