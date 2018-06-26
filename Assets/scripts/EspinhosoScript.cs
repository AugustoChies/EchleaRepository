using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EspinhosoScript : NetworkBehaviour {
    public GameObject hissarea, attackarea, explorer, bird;
    public float movespeed;
    public GameObject spine,corpse;
    float stuntimer;
    [SyncVar]
    bool direction;
    [SyncVar]
    public float attacktimer;
    [SyncVar]
    public bool stunned;

    Animator anim;
    // Use this for initialization
    void Start () {
        direction = true;
        attacktimer = 0;
        anim = this.gameObject.GetComponent<Animator>();
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
            anim.SetBool("hissing", false);
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            stuntimer += 1 * Time.deltaTime;
            if(stuntimer > 2)
            {
                stunned = false;
                stuntimer = 0;
                anim.SetBool("stunned", false);
                if (explorer != null)
                    explorer.GetComponent<Expmove>().DestunMe(this.gameObject);
                if (bird != null)
                    bird.GetComponent<Birdmove>().DestunMe(this.gameObject);
            }
        }
        else
        {
            if (attackarea.GetComponent<PlayerDetector>().detected)
            {
                anim.SetBool("hissing", false);
                if (attacktimer <= 0)
                {
                    anim.SetTrigger("attack");
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
                anim.SetBool("hissing",true);
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            else if (direction)
            {
                anim.SetBool("hissing", false);
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * movespeed, 0);
            }
            else
            {
                anim.SetBool("hissing", false);
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
                hissarea.transform.localPosition = new Vector3(6.14f, 2.1f);
                attackarea.transform.localPosition = new Vector3(3.8f, 0.62f);
            }
            else
            {
                hissarea.transform.localPosition = new Vector3(-6.14f, 2.1f);
                attackarea.transform.localPosition = new Vector3(-3.8f, 0.62f);
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
            anim.SetTrigger("stun");
            anim.SetBool("stunned", true);
            if (explorer != null)
                explorer.GetComponent<Expmove>().stunMe(this.gameObject);
            if (bird != null)
                bird.GetComponent<Birdmove>().stunMe(this.gameObject);
        }
    }
}
