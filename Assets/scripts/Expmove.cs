using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Expmove : NetworkBehaviour
{
    public bool canmove, canjump, onrope, touchingrope, dead;
    bool limited, amilocalplayer, amiserver;
    public float speed;
    public float jumpvalue;
    public float xrope;
    
    public float horizontal, vertical;
    public GameObject attack, scan, revival, myparent, bird, pointer;
    public float distresstimer;
    Transform maincamera;
   // GameObject actualrope;
    [SyncVar]
    public bool reviving;
    [SyncVar]
    public bool distress;

    public bool onfriendrevival;

    bool direction; //0 left, 1 right
    float scantimer, attacktimer;

    public float vspeed;
    // Use this for initialization
    void Start()
    {       
        maincamera = Camera.main.transform;
        canmove = true;
        attacktimer = scantimer = -1;
        distresstimer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!amilocalplayer)
        {
            return;
        }

        

        if (canmove)
        {
            if (onrope)
            {
                vertical = horizontal = 0;
                if (Input.GetKey("a"))
                {

                    this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    onrope = false;
                    this.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(-jumpvalue, jumpvalue * 0.7f);
                }
                else if (Input.GetKey("d"))
                {

                    this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    onrope = false;
                    this.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(jumpvalue, jumpvalue * 0.7f);
                }
                if (Input.GetKey("w"))
                {
                    if (!limited)
                        vertical = 1;
                }
                else if (Input.GetKey("s"))
                {
                    vertical = -1;
                }

                this.gameObject.GetComponent<Rigidbody2D>().MovePosition
                                        (gameObject.GetComponent<Rigidbody2D>().position + new Vector2(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime));
            }
            else if (Input.GetKey("a") || Input.GetKey("d"))
            {
                if (Input.GetKey("a"))
                {
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * speed, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                    direction = false;
                }
                else if (Input.GetKey("d"))
                {
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * speed, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                    direction = true;
                }
            }
            else
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }

        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
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
        if(bird == null)
        {
            bird = GameObject.Find("Bird(Clone)");
        }
        if (!amilocalplayer)
        {
            return;
        }
        maincamera.position = this.gameObject.GetComponent<Transform>().position + new Vector3(0, 0, -10);
        

        if (canmove)
        {
            if (Input.GetKeyDown("w"))
            {
                if (touchingrope && !onrope)
                {
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    onrope = true;
                    this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    this.gameObject.transform.position = new Vector3(xrope, this.gameObject.transform.position.y, 0);
                }
                else if (canjump)
                {
                    this.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0, jumpvalue);
                    canjump = false;
                }
            }

            if (Input.GetKeyDown("j") && canjump)
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
            if (Input.GetKeyDown("k") && canjump)
            {
                scantimer = 0;
                canmove = false;
            }         
        }

        if (Input.GetKeyDown("p"))
        {
            CmdDistress(true);
        }

        if (scantimer >= 0)
        {
            scantimer += 1 * Time.deltaTime;
            if (scantimer >= 5)
            {
                scan.GetComponent<BoxCollider2D>().enabled = false;
                scan.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
                scan.GetComponent<SpriteRenderer>().enabled = false;
                scantimer = -1;
                if (!dead)
                    canmove = true;
            }
            else if (scantimer >= 3)
            {
                if (!dead)
                {
                    scan.GetComponent<BoxCollider2D>().enabled = true;
                    scan.GetComponent<BoxCollider2D>().offset = new Vector2(0.005f, 0);
                    scan.GetComponent<SpriteRenderer>().enabled = true;
                }
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

        if (distress)
        {
            distresstimer += 1 * Time.deltaTime;
            if (distresstimer > 3f)
            {
                CmdDistress(false);
                distresstimer = 0;
            }
        }

        if (bird!= null && bird.GetComponent<Birdmove>().distress)
        {
            pointer.GetComponent<SpriteRenderer>().enabled = true;

            Vector3 targ = bird.transform.position;
            

            Vector3 objectPos = pointer.transform.position;
            targ = targ - objectPos;
            

            float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
        else
        {
            pointer.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (reviving)
        {
            if (!onfriendrevival || dead)
                reviving = false;
        }

        if(dead)
        {
            if(bird.GetComponent<Birdmove>().reviving)
            {
                CmdLifeStatus(false);
                CmdDistress(false);
            }
        }

        vspeed = this.gameObject.GetComponent<Rigidbody2D>().velocity.y;
    }

    

    void OnTriggerStay2D(Collider2D other)
    {
        if (amilocalplayer)
        {
            if (other.gameObject.tag == "corda")
            {
                if (dead)
                {
                    touchingrope = false;
                    this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    onrope = false;
                }
                else
                {
                    touchingrope = true;
                }
                xrope = other.gameObject.transform.position.x;
            }
            if (other.gameObject.tag == "limit")
            {
                limited = true;
            }
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
            if (other.gameObject.tag == "curse")
            {
                myparent.GetComponent<PlayerObjsScript>().Lose();
            }
            if (other.gameObject.tag == "enemy")
            {
                CmdLifeStatus(true);
                CmdDistress(true);
            }
            if (other.gameObject.tag == "door")
            {                
                if (myparent.GetComponent<PlayerObjsScript>().ReturnKeys() >= 3)
                {                    
                    if (Input.GetKey("w"))
                    {
                        myparent.GetComponent<PlayerObjsScript>().Win();
                    }
                }
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "corda")
        {
            touchingrope = false;
        }
        if (other.gameObject.tag == "limit")
        {
            limited = false;
        }
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
                CmdDistress(true);
            }

            if (coll.gameObject.tag == "chao" && vspeed <= -12)
            {
                CmdLifeStatus(true);
                CmdDistress(true);
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
            onrope = false;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
        else
        {
            revival.GetComponent<BoxCollider2D>().enabled = false;
            dead = isitdead;
            canmove = true;
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

    [ClientRpc]
    void RpcDistress(bool emmmiting)
    {
        distress = emmmiting;
        distresstimer = 0;
    }

    [Command]
    void CmdDistress(bool emmmiting)
    {
        distress = emmmiting;
        RpcDistress(emmmiting);
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
            myparent.GetComponent<PlayerObjsScript>().UpdadeInventory(caller,type);
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
            myparent.GetComponent<PlayerObjsScript>().ShootSpine(caller,direction);
    }

    public void DropBall(GameObject caller)
    {
        if (amiserver && amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().DropBall(caller);
    }

    public void ChangeClaw(GameObject caller, bool direction, bool attacking)
    {
        if (amiserver && amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().ChangeClaw(caller,direction,attacking);
    }

    public void CallCurse(GameObject caller)
    {        
        if (amiserver && amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().CallCurse(caller);
    }

    
}