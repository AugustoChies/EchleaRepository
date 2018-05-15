using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Birdmove : NetworkBehaviour {
    public bool canmove, dead;
    public float speed;
    bool amilocalplayer, amiserver;
    int horizontal, vertical;
    public GameObject rope, attack, scan, revival, myparent, explorer;
    float restimer;
    Transform maincamera;
    [SyncVar]
    public bool reviving;

    public bool onfriendrevival;

    bool direction; //0 left, 1 right
    float scantimer, attacktimer;

   
    // Use this for initialization
    void Start () {
               
        maincamera = Camera.main.transform;
        canmove = true;
        attacktimer = scantimer = -1;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!amilocalplayer)
        {
            return;
        }
        
        vertical = horizontal = 0;
        
        if (canmove)
        {
            if (Input.GetKey("a"))
            {
                horizontal = -1;
                direction = false;
            }
            else if (Input.GetKey("d"))
            {
                horizontal = 1;
                direction = true;
            }
            if (Input.GetKey("w"))
            {
                vertical = 1;
            }
            else if(Input.GetKey("s"))
            {
                vertical = -1;
            }

            this.gameObject.GetComponent<Rigidbody2D>().MovePosition
                                    (gameObject.GetComponent<Rigidbody2D>().position + new Vector2(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime));
            
        }

    }

    void Update()
    {
        if (myparent == null)
        {
            if (transform.parent != null)
            {
                Transform pttrans = transform.parent;
                myparent = pttrans.gameObject;
                amiserver = myparent.GetComponent<PlayerObjsScript>().AmIServer();
                amilocalplayer = myparent.GetComponent<PlayerObjsScript>().AmILocal();
            }
            else
            {
                amiserver = false;
                amilocalplayer = false;
            }
        }
        if(explorer == null)
        {
            explorer = GameObject.Find("Explorer(Clone)");
        }
        if (!amilocalplayer)
        {
            return;
        }
        maincamera.position = this.gameObject.GetComponent<Transform>().position + new Vector3(0, 0, -10);
        
        if (canmove)
        {
            if (Input.GetKeyDown("l"))
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                canmove = false;
                myparent.GetComponent<PlayerObjsScript>().CallRope(this.gameObject);
            }
            if (Input.GetKeyDown("j"))
            {
                attack.GetComponent<BoxCollider2D>().enabled = true;
                attack.GetComponent<SpriteRenderer>().enabled = true;
                if (direction)
                {
                    attack.GetComponent<BoxCollider2D>().offset = new Vector2(0.6f, 0);
                }
                else
                {
                    attack.GetComponent<BoxCollider2D>().offset = new Vector2(-0.6f, 0);
                }
                attacktimer = 0;
            }
            if (Input.GetKeyDown("k"))
            {
                scantimer = 0;
                scan.GetComponent<CapsuleCollider2D>().enabled = true;
                scan.GetComponent<CapsuleCollider2D>().offset = new Vector2(0.005f, 0);
                scan.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            if (!dead)
            {
                if (Input.GetKeyDown("l"))
                {
                    myparent.GetComponent<PlayerObjsScript>().DestroyRope();
                    canmove = true;
                }
            }
        }


        if (scantimer >= 0)
        {
            scantimer += 1 * Time.deltaTime;
            if (scantimer >= 1)
            {
                scan.GetComponent<CapsuleCollider2D>().enabled = false;
                scan.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 0);
                scan.GetComponent<SpriteRenderer>().enabled = false;
                scantimer = -1;
            }           
        }


        if (attacktimer >= 0)
        {
            attacktimer += 1 * Time.deltaTime;
            if (attacktimer > 0.7f)
            {
                attack.GetComponent<BoxCollider2D>().enabled = false;
                attack.GetComponent<SpriteRenderer>().enabled = false;
                attacktimer = -1;
            }
        }

        if (dead)
        {
            restimer += 1 * Time.deltaTime;
            if (restimer > 10)
            {
                restimer = 0;
                CmdLifeStatus(false);
            }
            if (explorer.GetComponent<Expmove>().reviving)
            {
                restimer = 0;
                CmdLifeStatus(false);
            }
        }

        if (reviving)
        {
            if (!onfriendrevival || dead)
                reviving = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (amilocalplayer)
        {            
            if (other.gameObject.tag == "revivalarea")
            {
                if (!dead)
                {
                    onfriendrevival = true;
                    if (Input.GetKey("r"))
                    {
                        CmdIAmReviving();
                    }
                }
            }
            if (other.gameObject.tag == "enemy")
            {
                CmdLifeStatus(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {        
        if (other.gameObject.tag == "revivalarea")
        {
            onfriendrevival = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        
            if (coll.gameObject.tag == "enemyattack")
            {
                 CmdLifeStatus(true);
                 this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 0));
            }
            if (coll.gameObject.tag == "chao")
            {
                if (dead)
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        
    }



    [ClientRpc]
    void RpcLifeStatus(bool isitdead)
    {
        if (isitdead)
        {
            revival.GetComponent<BoxCollider2D>().enabled = true;
            dead = isitdead;
            canmove = false;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            myparent.GetComponent<PlayerObjsScript>().DestroyRope();
        }
        else
        {
            revival.GetComponent<BoxCollider2D>().enabled = false;
            dead = isitdead;
            canmove = true;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    [Command]
    public void CmdLifeStatus(bool isitdead)
    {

        RpcLifeStatus(isitdead);
    }

    [Command]
    void CmdIAmReviving()
    {
        reviving = true;
    }

    //COMANDOS DE TERCEIROS-------------------------------------------------------------------------------------------------

    public void ShowIcon(GameObject caller)
    {
        if (amilocalplayer)
        {
            myparent.GetComponent<PlayerObjsScript>().ShowIcon(caller);
        }
    }

    public void Dig(GameObject caller)
    {
        if (amilocalplayer)
        {
            myparent.GetComponent<PlayerObjsScript>().Dig(caller);
        }
    }

    public void UpdadeInventory(GameObject caller, int type)
    {
        if (amilocalplayer)
        {
            myparent.GetComponent<PlayerObjsScript>().UpdadeInventory(caller, type);
        }
    }

    public void DestroyMe(GameObject caller)
    {
        if (amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().DestroyMe(caller);
    }

    public void stunMe(GameObject caller)
    {
        if (amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().stunMe(caller);
    }

    public void ShootSpine(GameObject caller, bool direction)
    {
        if (amiserver && amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().ShootSpine(caller, direction);
    }

    public void DropBall(GameObject caller)
    {
        if (amiserver && amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().DropBall(caller);
    }

    public void ChangeClaw(GameObject caller, bool direction, bool attacking)
    {
        if (amiserver && amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().ChangeClaw(caller, direction, attacking);
    }

    public void CallCurse(GameObject caller)
    {
        if (amiserver && amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().CallCurse(caller);
    }
}
