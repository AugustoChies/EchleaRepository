using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OwlScript : NetworkBehaviour {
    public GameObject attackarea, explorer, bird;
    public GameObject spine;
    float stuntimer;
    [SyncVar]
    public float attacktimer;
    [SyncVar]
    public bool stunned;
    // Use this for initialization
    void Start()
    {
        attacktimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (explorer == null)
        {
            explorer = GameObject.Find("Explorer(Clone)");
        }

        if (bird == null)
        {
            bird = GameObject.Find("Bird(Clone)");
        }
        if (stunned)
        {            
            stuntimer += 1 * Time.deltaTime;
            if (stuntimer > 2)
            {
                stunned = false;
                stuntimer = 0;
            }
        }
        else
        {
            if (attackarea.GetComponent<PlayerDetector>().detected)
            {
                if (attacktimer <= 0)
                {
                    if (explorer != null)
                        explorer.GetComponent<Expmove>().DropBall(this.gameObject);
                    if (bird != null)
                        bird.GetComponent<Birdmove>().DropBall(this.gameObject);
                    attacktimer = 2.5f;
                }
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }

        if (attacktimer > 0)
        {
            attacktimer -= 1 * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "ataqueex")
        {
            if (explorer != null)
                explorer.GetComponent<Expmove>().DestroyMe(this.gameObject);
            if (bird != null)
                bird.GetComponent<Birdmove>().DestroyMe(this.gameObject);
        }
        if (other.gameObject.tag == "ataquebi")
        {
            if (explorer != null)
                explorer.GetComponent<Expmove>().stunMe(this.gameObject);
            if (bird != null)
                bird.GetComponent<Birdmove>().stunMe(this.gameObject);
        }
    }
}