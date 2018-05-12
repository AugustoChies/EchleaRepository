using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EspinhosoScript : NetworkBehaviour {
    public GameObject hissarea, attackarea, explorer, bird;
    public float movespeed;
    public GameObject spine;
    float stuntimer;
    [SyncVar]
    bool direction;
    [SyncVar]
    public float attacktimer;
    [SyncVar]
    public bool stunned;
	// Use this for initialization
	void Start () {
        direction = true;
        attacktimer = 0;
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
        if (stunned)
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            stuntimer += 1 * Time.deltaTime;
            if(stuntimer > 2)
            {
                stunned = false;
                stuntimer = 0;
            }
        }
        else
        {
            if (attackarea.GetComponent<PlayerDetector>().detected)
            {
                if(attacktimer <= 0)
                {
                    if (explorer != null)
                        explorer.GetComponent<Expmove>().ShootSpine(this.gameObject,direction);
                    if (bird != null)
                        bird.GetComponent<Birdmove>().ShootSpine(this.gameObject, direction);
                    attacktimer = 1.5f;
                }
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            else if (hissarea.GetComponent<PlayerDetector>().detected)
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            else if (direction)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * movespeed, 0);
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * movespeed, 0);
            }
        }

        if(attacktimer > 0)
        {
            attacktimer -= 1 * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "monsterlimit")
        {
            direction = !direction;
            
            if (direction)
            {
                hissarea.transform.localPosition = new Vector3(3.15f, 0);
                attackarea.transform.localPosition = new Vector3(1.7f, 0);
            }
            else
            {
                hissarea.transform.localPosition = new Vector3(-3.15f, 0);
                attackarea.transform.localPosition = new Vector3(-1.7f, 0);
            }
        }

        if(other.gameObject.tag == "ataqueex")
        {
            if(explorer != null)
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
