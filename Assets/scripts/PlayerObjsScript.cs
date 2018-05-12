using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerObjsScript : NetworkBehaviour {
    public GameObject mychar,instanced;
    [SyncVar]
    private GameObject objectID;
    bool started;
    [SyncVar]
    public GameObject inventory;
    GameObject actualrope;

    public GameObject canvas;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!started)
        {
            if (SceneManager.GetActiveScene().name == "Stage1")
            {
                started = true;
                CmdSpawnPlayer();
            }           
        }
        if (this.transform.childCount > 1)
        {
            if(transform.Find("Bird(Clone)"))
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

        if(instanced == null)
        {
            if(mychar.name == "Explorer")
            {
                instanced = GameObject.Find("Explorer(Clone)");
                instanced.transform.SetParent(this.transform);
            }
            else if (mychar.name == "Bird")
            {
                instanced = GameObject.Find("Bird(Clone)");
                instanced.transform.SetParent(this.transform);
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
        caller.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        caller.GetComponent<EspinhosoScript>().stunned = true;
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
