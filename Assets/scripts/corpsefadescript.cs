using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corpsefadescript : MonoBehaviour {
    Animator anim;
    float time;
    bool fade;
	// Use this for initialization
	void Start () {
        time = 0;
        anim = this.gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(fade)
        {
            gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1 * Time.deltaTime);
            time += Time.deltaTime;
            if(time > 1)
            {
                Destroy(this.gameObject);
            }
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("dead"))
        {
            fade = true;
        }

    }
}
