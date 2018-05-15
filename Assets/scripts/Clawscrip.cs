using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Clawscrip : NetworkBehaviour {
    public GameObject attackarea, explorer, bird;
    public float movespeed;
    float stuntimer;
    [SyncVar]
    public bool direction;
    [SyncVar]
    public float attacktimer;
    [SyncVar]
    public bool stunned;
    [SyncVar]
    public bool attacking;
    // Use this for initialization
    void Start()
    {
        direction = true;
        attacktimer = 0;
        attacking = false;
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
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            stuntimer += 1 * Time.deltaTime;
            if (stuntimer > 2)
            {
                stunned = false;
                stuntimer = 0;
            }
        }
        else
        {
            if (!attacking)
            {
                if (attackarea.GetComponent<PlayerDetector>().detected)
                {
                    if (attacktimer <= 0)
                    {
                        attacking = true;
                        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                        float enemypos = attackarea.GetComponent<PlayerDetector>().xpos;

                        if(enemypos >= this.transform.position.x)
                        {
                            direction = true;
                        }
                        else
                        {
                            direction = false;
                        }

                        if (explorer != null)
                            explorer.GetComponent<Expmove>().ChangeClaw(this.gameObject, direction,attacking);
                        if (bird != null)
                            bird.GetComponent<Birdmove>().ChangeClaw(this.gameObject, direction, attacking);

                        attacktimer = 5f;
                    }
                    
                }
            }
            else
            { 
                if (direction)
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
   

        }

        if (attacktimer > 0)
        {
            attacktimer -= 1 * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "monsterlimit")
        {
            attacking = false;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            if(direction)
            {
                this.transform.Translate(-0.5f, 0, 0);
            }
            else
            {
                this.transform.Translate(0.5f, 0, 0);
            }

            if (explorer != null)
                explorer.GetComponent<Expmove>().ChangeClaw(this.gameObject, direction, attacking);
            if (bird != null)
                bird.GetComponent<Birdmove>().ChangeClaw(this.gameObject, direction, attacking);
        }

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
