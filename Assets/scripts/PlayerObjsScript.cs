using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerObjsScript : NetworkBehaviour {
    public GameObject mychar,instanced;
    [SyncVar]
    private GameObject objectID;
    public bool started;
    [SyncVar]
    public GameObject inventory;
    GameObject actualrope;
    public GameObject canvas;

    //coisas do char select
    public GameObject selectcanvas,selectcontroller;
    public bool didIClick;

    // Use this for initialization
    void Start () {
        started = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!started)
        {
            if (isLocalPlayer)
            {
                started = true;
                if (SceneManager.GetActiveScene().name == "Stage1")
                {                    
                    CmdSpawnPlayer();
                }
            }
        }
        if (started)
        {
            if(SceneManager.GetActiveScene().name == "CharacterSelect")
            {
                if (selectcanvas == null)
                    selectcanvas = GameObject.Find("SCanvas");
                if (selectcontroller == null)
                    selectcontroller = GameObject.Find("ChangeController");
                if(selectcontroller.GetComponent<SwapNetwork>().mypobject == null)
                {
                    selectcontroller.GetComponent<SwapNetwork>().mypobject = this.gameObject;
                }

                if(selectcontroller.GetComponent<SwapNetwork>().clicked && didIClick)
                {
                    selectcanvas.GetComponent<MenuPlayScript>().ShowWait();
                }
            }
            if (SceneManager.GetActiveScene().name == "Stage1")
            {
                if (this.transform.childCount > 1)
                {
                    if (transform.Find("Bird(Clone)"))
                    {
                        Transform shine = transform.Find("Bird(Clone)");
                        Destroy(shine.gameObject);
                    }
                    if (transform.Find("Explorer(Clone)"))
                    {
                        Transform shine = transform.Find("Explorer(Clone)");
                        Destroy(shine.gameObject);
                    }
                }
                if (canvas == null)
                    canvas = GameObject.Find("Statistics");

                if (instanced == null)
                {
                    if (mychar.name == "Explorer")
                    {
                        instanced = GameObject.Find("Explorer(Clone)");
                        // instanced.transform.SetParent(this.transform);
                    }
                    else if (mychar.name == "Bird")
                    {
                        instanced = GameObject.Find("Bird(Clone)");
                        // instanced.transform.SetParent(this.transform);
                    }
                }
            }
        }
    }

    public bool AmIServer()
    {
        return isServer;
    }

    public bool AmILocal()
    {
        return isLocalPlayer;
    }

    public int ReturnKeys()
    {
        return canvas.GetComponent<CanvasScript>().keys;
    }

    [ClientRpc]
    void RpcUpdateSpawned(GameObject inst,GameObject parent)
    {
        instanced = inst;
        inst.transform.SetParent(parent.transform);
    }


    [Command]
    void CmdSpawnPlayer()
    {
        instanced = GameObject.Instantiate(mychar, this.gameObject.transform.position, Quaternion.identity);
        instanced.transform.parent = this.transform;
        NetworkServer.SpawnWithClientAuthority(instanced, this.gameObject);
        RpcUpdateSpawned(instanced, this.gameObject);
    }

    //COMMANDS_______________________________________________________________________________________________
    public void UpdateClicked(GameObject caller,bool value)
    {
        if (isLocalPlayer)
        {
            objectID = caller;
            didIClick = value;
            CmdUpdateClicked(objectID, value);
        }
    }


    public void ShowIcon(GameObject caller)
    {
        objectID = caller;
        
        CmdShowIcon(objectID);
        
    }

    public void Dig(GameObject caller)
    {
        objectID = caller;
        
            CmdDug(objectID);
        
    }

    public void UpdadeInventory(GameObject caller, int type)
    {
        inventory = caller;
       
            CmdUpdateInventory(inventory, type);

        objectID = canvas;

        CmdUpdateCanvas(objectID, type);
        
    }

    public void DestroyMe(GameObject caller)
    {
        objectID = caller;
       
            CmdDestroy(objectID);
    }

    public void stunMe(GameObject caller)
    {
        objectID = caller;
        
            CmdStun(objectID);
    }

    public void ShootSpine(GameObject caller, bool direction)
    {
        objectID = caller;
        
            CmdShootSpine(caller, direction);
    }

    public void ChangeClaw(GameObject caller, bool direction, bool attacking)
    {
        objectID = caller;

        CmdChangeSandClaw(caller, direction,attacking);
    }

    public void DropBall(GameObject caller)
    {
        objectID = caller;

        CmdDropBall(caller);
    }

    public void CallCurse(GameObject caller)
    {
        objectID = caller;
       
            CmdCurse(caller);
    }

    public void Lose()
    {        
        CmdLose();
    }

    public void Win()
    {
        CmdLose();
    }

    public void CallRope(GameObject caller)
    {
        CmdSpawnRope(caller);
    }

    public void DestroyRope()
    {
        CmdDestroyRope();
    }

    [Command]
    void CmdUpdateClicked(GameObject caller,bool value)
    {
        NetworkIdentity objNetId = caller.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        caller.GetComponent<SwapNetwork>().clicked = value;
        objNetId.RemoveClientAuthority(connectionToClient);
    }


    [Command]
    void CmdSpawnRope(GameObject caller)
    {
        GameObject instance = Instantiate(caller.GetComponent<Birdmove>().rope, caller.transform.position,  Quaternion.identity);
        instance.GetComponent<Ropescript>().startingpoint = caller.transform.position;
        actualrope = instance;
        NetworkServer.Spawn(instance);
    }

    [Command]
    void CmdDestroyRope()
    {
        NetworkServer.Destroy(actualrope);
        actualrope = null;
    }

    [Command]
    void CmdShowIcon(GameObject caller)
    {
        GameObject instance = Instantiate(caller.GetComponent<digsitescript>().icon, caller.transform.position + new Vector3(0, 2, -1), Quaternion.identity);
        NetworkServer.Spawn(instance);
    }

    [Command]
    void CmdDug(GameObject caller)
    {
        NetworkIdentity objNetId = caller.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        caller.GetComponent<digsitescript>().dug = true;
        GameObject instance = Instantiate(caller.GetComponent<digsitescript>().icon, caller.transform.position + new Vector3(0, 2, -1), Quaternion.identity);
        NetworkServer.Spawn(instance);
        objNetId.RemoveClientAuthority(connectionToClient);

    }

    [Command]
    void CmdUpdateInventory(GameObject caller, int type)
    {

        NetworkIdentity objNetId = caller.GetComponent<NetworkIdentity>();

        objNetId.AssignClientAuthority(connectionToClient);
        switch (type)
        {
            case 0:
                caller.GetComponent<Inventoryscr>().keys += 1;
                break;

            default:
                break;
        }
        objNetId.RemoveClientAuthority(connectionToClient);
    }

    [Command]
    void CmdUpdateCanvas(GameObject caller, int type)
    {
        NetworkIdentity objNetId = caller.GetComponent<NetworkIdentity>();

        objNetId.AssignClientAuthority(connectionToClient);
        canvas.GetComponent<CanvasScript>().curserate += 0.1f;
        if (type == -2)
        {
            canvas.GetComponent<CanvasScript>().curserate += 0.1f;
        }
        objNetId.RemoveClientAuthority(connectionToClient);
         
    }

    [Command]
    void CmdDestroy(GameObject caller)
    {
        NetworkServer.Destroy(caller);
    }

    [Command]
    void CmdStun(GameObject caller)
    {
        NetworkIdentity objNetId = caller.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        
        if (caller.GetComponent<EspinhosoScript>())
        {
            caller.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            caller.GetComponent<EspinhosoScript>().stunned = true;
        }
        else if (caller.GetComponent<OwlScript>())
        {
            caller.GetComponent<OwlScript>().stunned = true;
        }
        else if (caller.GetComponent<Clawscrip>())
        {
            caller.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            caller.GetComponent<Clawscrip>().stunned = true;
        }
        objNetId.RemoveClientAuthority(connectionToClient);
    }

    [Command]
    void CmdShootSpine(GameObject caller, bool direction)
    {
        NetworkIdentity objNetId = caller.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        caller.GetComponent<EspinhosoScript>().attacktimer = 1.5f;
        GameObject instance = Instantiate(caller.GetComponent<EspinhosoScript>().spine, caller.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        if (direction)
            instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(700, 100));
        else
            instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(-700, 100));
        NetworkServer.Spawn(instance);
        objNetId.RemoveClientAuthority(connectionToClient);
    }

    [Command]
    void CmdDropBall(GameObject caller)
    {
        NetworkIdentity objNetId = caller.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        caller.GetComponent<OwlScript>().attacktimer = 2.5f;
        GameObject instance1 = Instantiate(caller.GetComponent<OwlScript>().spine, caller.transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
        GameObject instance2 = Instantiate(caller.GetComponent<OwlScript>().spine, caller.transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
        
        instance1.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(1,100), Random.Range(50,200)));
        instance2.GetComponent<Rigidbody2D>().AddForce(new Vector2(-Random.Range(1, 100), Random.Range(50, 200)));
        NetworkServer.Spawn(instance1);
        NetworkServer.Spawn(instance2);
        objNetId.RemoveClientAuthority(connectionToClient);
    }

    [ClientRpc]
    void RpcUpdateSandEnabled(GameObject caller, bool attacking)
    {
        if(attacking)
        {
            caller.GetComponent<SpriteRenderer>().enabled = true;
            caller.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            caller.GetComponent<SpriteRenderer>().enabled = false;
            caller.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    [Command]
    void CmdChangeSandClaw(GameObject caller,bool direction, bool attacking)
    {
        NetworkIdentity objNetId = caller.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        
        caller.GetComponent<Clawscrip>().direction = direction;



        if(attacking)
        {
            caller.GetComponent<Clawscrip>().attacktimer = 5f;
            caller.GetComponent<Clawscrip>().attacking = true;
            caller.GetComponent<SpriteRenderer>().enabled = true;
            caller.GetComponent<BoxCollider2D>().enabled = true;
            RpcUpdateSandEnabled(caller, attacking);
        }
        else
        {
            if (direction)
            {
                this.transform.Translate(-0.5f, 0, 0);
            }
            else
            {
                this.transform.Translate(0.5f, 0, 0);
            }
            caller.GetComponent<Clawscrip>().attacking = false;
            caller.GetComponent<SpriteRenderer>().enabled = false;
            caller.GetComponent<BoxCollider2D>().enabled = false;
            caller.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            RpcUpdateSandEnabled(caller, attacking);
        }


        objNetId.RemoveClientAuthority(connectionToClient);
    }

    [Command]
    void CmdCurse(GameObject caller)
    {
        GameObject instance = Instantiate(caller.GetComponent<CanvasScript>().curseobj, new Vector3(0, 142, -5), new Quaternion(0, 0, 0, 1));

        NetworkServer.Spawn(instance);
    }

    [Command]
    void CmdLose()
    {
        canvas.GetComponent<CanvasScript>().lost = true;
    }

    [Command]
    void CmdWin()
    {
        canvas.GetComponent<CanvasScript>().won = true;
    }
}
