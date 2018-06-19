using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Birdmove : NetworkBehaviour {
    public bool canmove, dead;
    public float speed;
    bool amilocalplayer, amiserver;
    int horizontal, vertical;
    public GameObject rope, attack, scan, revival, myparent, explorer,load, inventory, pointer;
    float restimer;
    public float distresstimer;
    Transform maincamera;
    [SyncVar]
    public bool reviving;
    [SyncVar]
    public bool carrying;
    [SyncVar]
    public bool distress;
    [SyncVar]
    public int relicindex;//0 é chave

    public bool onfriendrevival;

    [SyncVar]
    bool direction; //0 left, 1 right
    bool oldirection;
    float scantimer, attacktimer;
    GameObject spawn;

    Animator anim;
    // Use this for initialization
    void Start () {
               
        maincamera = Camera.main.transform;
        canmove = true;
        attacktimer = scantimer = -1;
        relicindex = 0;
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
        if(explorer == null)
        {
            explorer = GameObject.Find("Explorer(Clone)");
        }
        if (inventory == null)
        {
            inventory = GameObject.Find("InventoryInfo");
        }
        if (carrying)
        {
            load.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            load.GetComponent<SpriteRenderer>().enabled = false;
        }
        if(spawn == null)
        {
            spawn = GameObject.Find("SpawnBird");
            this.transform.position = spawn.transform.position;
        }
        if (direction)
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
            if (Input.GetKeyDown("l"))
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                canmove = false;
                anim.SetBool("Deployed", true);
                CmdAttAnim(5);
                myparent.GetComponent<PlayerObjsScript>().CallRope(this.gameObject);
            }
            if (Input.GetKeyDown("j"))
            {
                attack.GetComponent<BoxCollider2D>().enabled = true;

                CmdAttAnim(1);
                if (direction)
                {
                    attack.transform.localPosition = new Vector3(0.75f, -0.35f, 0);
                }
                else
                {
                    attack.transform.localPosition = new Vector3(-0.75f, -0.35f, 0);
                }
                attacktimer = 0;
            }
            if (Input.GetKeyDown("k"))
            {
                scantimer = 0;
                scan.GetComponent<CapsuleCollider2D>().enabled = true;
                scan.GetComponent<CapsuleCollider2D>().offset = new Vector2(0.005f, 0);
                CmdAttAnim(2);
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
                    anim.SetBool("Deployed", false);
                }
            }
        }

        if (Input.GetKeyDown("p"))
        {
            CmdDistress(true);
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

        if (distress)
        {
            distresstimer += 1 * Time.deltaTime;
            if (distresstimer > 3f)
            {
                CmdDistress(false);
                distresstimer = 0;
            }
        }

        if (explorer != null && explorer.GetComponent<Expmove>().distress)
        {
            pointer.GetComponent<SpriteRenderer>().enabled = true;

            Vector3 targ = explorer.transform.position;


            Vector3 objectPos = pointer.transform.position;
            targ = targ - objectPos;


            float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            pointer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
        else
        {
            pointer.GetComponent<SpriteRenderer>().enabled = false;
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
                CmdDistress(false);
                anim.SetBool("Dead",false);
                CmdAttAnim(4);
                
            }
            if (explorer != null && explorer.GetComponent<Expmove>().reviving)
            {
                restimer = 0;
                CmdLifeStatus(false);
                CmdDistress(false);
                anim.SetBool("Dead", false);
                CmdAttAnim(4);
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
                CmdDistress(true);
                anim.SetBool("Dead", true);
                CmdAttAnim(3);
            }
            if (other.gameObject.tag == "delivery")
            {
                if (carrying)
                {
                    inventory.GetComponent<Inventoryscr>().sendInfo(relicindex);
                    carrying = false;
                    CmdCarrying(false, 0);
                }
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
                 CmdDistress(true);
                anim.SetBool("Dead", true);
                CmdAttAnim(3);

                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 0));
            }
            if (coll.gameObject.tag == "chao")
            {
                if (dead)
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        
    }

    public void Changecarrying(bool carring, int relindex)
    {        
        if (amilocalplayer)
        {           
            carrying = carring;
            relicindex = relindex;
            CmdCarrying(carring, relindex);
        }
    }

    [Command]
    void CmdCarrying(bool carr, int ri)
    {
        carrying = carr;
        relicindex = ri;
    }

    [Command]
    public void CmdChangeDir(bool dir)
    {
        direction = dir;
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
            case 5:
                anim.SetTrigger("Deploy");
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
    void RpcLifeStatus(bool isitdead)
    {
        if (isitdead)
        {
            revival.GetComponent<BoxCollider2D>().enabled = true;
            dead = isitdead;
            canmove = false;
            anim.SetBool("Deployed", false);
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
