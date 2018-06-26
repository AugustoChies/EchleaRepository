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

    [SyncVar]
    bool direction; //0 left, 1 right
    bool oldirection;
    float scantimer, attacktimer;
    GameObject spawn;
    public float vspeed;

    Animator anim;
    // Use this for initialization
    void Start()
    {       
        maincamera = Camera.main.transform;
        canmove = true;
        attacktimer = scantimer = -1;
        distresstimer = 0;
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {       
        if (!amilocalplayer)
        {
            return;
        }

        oldirection = direction;

        if (canmove)
        {
            if (onrope)
            {
                vertical = horizontal = 0;
                if (Input.GetKey("a"))
                {                    
                    this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    onrope = false;
                    anim.SetBool("Onrope", false);
                    this.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(-jumpvalue, jumpvalue * 0.7f);
                }
                else if (Input.GetKey("d"))
                {

                    this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                    onrope = false;
                    anim.SetBool("Onrope", false);
                    this.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(jumpvalue, jumpvalue * 0.7f);
                }
                if (Input.GetKey("w"))
                {
                    if (!limited)
                    {
                        vertical = 1;
                        anim.SetBool("Moving", true);
                    }
                }
                else if (Input.GetKey("s"))
                {
                    vertical = -1;
                    anim.SetBool("Moving", true);
                }
                else
                {
                    anim.SetBool("Moving", false);
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
                    anim.SetBool("Moving", true);
                }
                else if (Input.GetKey("d"))
                {
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * speed, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                    direction = true;
                    anim.SetBool("Moving", true);
                }
            }
            else
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                anim.SetBool("Moving", false);
            }

        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }
        if (oldirection != direction)
        {
            CmdChangeDir(direction);
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
        if (spawn == null)
        {
            spawn = GameObject.Find("SpawnExplorer");
            this.transform.position = spawn.transform.position;
        }
        if(direction)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
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
                    anim.SetBool("Onrope", true);
                    this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;                    
                    this.gameObject.transform.position = new Vector3(xrope, this.gameObject.transform.position.y, 0);
                    if (direction)
                    {
                        this.gameObject.transform.position -= new Vector3(0.3f, 0, 0);
                    }
                    else
                    {
                        this.gameObject.transform.position -= new Vector3(-0.3f, 0, 0);
                    }
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
                
                CmdAttAnim(1);
                if (direction)
                {
                    attack.transform.localPosition = new Vector3(1.63f, -0.3f, 0);
                }
                else
                {
                    attack.transform.localPosition = new Vector3(-1.63f, -0.3f, 0);
                }
                attacktimer = 0;
            }
            if (Input.GetKeyDown("k") && canjump)
            {
                scantimer = 0;
                CmdAttAnim(2);
                canmove = false;
                anim.SetBool("Moving", false);
            }         
        }

        if (Input.GetKeyDown("p"))
        {
            CmdDistress(true);
        }

        if (scantimer >= 0)
        {
            scantimer += 1 * Time.deltaTime;
            if (scantimer >= 2.3)
            {
                scan.GetComponent<BoxCollider2D>().enabled = false;
                scan.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
                
                scantimer = -1;
                if (!dead)
                    canmove = true;
            }
            else if (scantimer >= 1.5)
            {
                if (!dead)
                {
                    scan.GetComponent<BoxCollider2D>().enabled = true;
                    scan.GetComponent<BoxCollider2D>().offset = new Vector2(0.005f, 0);
                    
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
                CmdIAmReviving(false);
        }

        if(dead)
        {
            if(bird != null && bird.GetComponent<Birdmove>().reviving)
            {
                anim.SetBool("Dead",false);
                CmdAttAnim(4);
                CmdLifeStatus(false);
                CmdDistress(false);
            }
        }

        vspeed = this.gameObject.GetComponent<Rigidbody2D>().velocity.y;

        if (!canjump && !onrope)
        {
            anim.SetBool("Midair", true);
        }
        else
        {
            anim.SetBool("Midair", false);
        }
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
                    anim.SetBool("Onrope", false);
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
                      CmdIAmReviving(true);
                    }
                }
            }
            if (other.gameObject.tag == "curse")
            {
                myparent.GetComponent<PlayerObjsScript>().Lose();
            }
            if (other.gameObject.tag == "enemy")
            {
                if (other.gameObject.GetComponent<EspinhosoScript>())
                {
                    if (other.gameObject.GetComponent<EspinhosoScript>().stunned)
                        return;
                }
                else if (other.gameObject.GetComponent<OwlScript>())
                {
                    if (other.gameObject.GetComponent<OwlScript>().stunned)
                        return;
                }
                else if (other.gameObject.GetComponent<Clawscrip>())
                {
                    if (other.gameObject.GetComponent<Clawscrip>().stunned)
                        return;
                }
                anim.SetBool("Dead", true);
                CmdAttAnim(3);
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
        if (amilocalplayer)
        {
            if (other.gameObject.tag == "corda")
            {
                touchingrope = false;
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                onrope = false;
                anim.SetBool("Onrope", false);
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
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (amilocalplayer)
        {

            if (coll.gameObject.tag == "enemyattack")
            {
                anim.SetBool("Dead", true);
                CmdAttAnim(3);
                CmdLifeStatus(true);
                CmdDistress(true);
            }

            if (coll.gameObject.tag == "chao" && vspeed <= -9)
            {
                anim.SetBool("Dead", true);
                CmdAttAnim(3);
                CmdLifeStatus(true);
                CmdDistress(true);
            }
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
            anim.SetBool("Moving", false);
            onrope = false;
            anim.SetBool("Onrope", false);
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
    public void CmdChangeDir(bool dir)
    {
        direction = dir;
    }

    [Command]
    void CmdIAmReviving(bool yes)
    {
        reviving = yes;
    }

    [ClientRpc]
    void RpcAttAnim(int animtype)
    {
        switch (animtype)
        {
            case 1:
                anim.SetTrigger("Attack");
                break;
            case 2:
                anim.SetTrigger("Scan");
                break;
            case 3:
                anim.SetTrigger("Die");
                break;
            case 4:
                anim.SetTrigger("Revive");
                break;
            default:
                break;
        }
    }

    [Command]
    void CmdAttAnim(int animtype)
    {
        RpcAttAnim(animtype);
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

    public void DestunMe(GameObject caller)
    {
        if (amilocalplayer)
            myparent.GetComponent<PlayerObjsScript>().DestunMe(caller);
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