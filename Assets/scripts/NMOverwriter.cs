using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NMOverwriter : NetworkManager
{
    public GameObject explorer, bird, playerobj;
    public GameObject char1, char2;
    GameObject player;
    GameObject player2;
    int spawncount = 0;

    


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        player = playerobj;
        player2 = playerobj;
        if (spawncount % 2 == 0)
        {
            player.GetComponent<PlayerObjsScript>().mychar = char1;
            player = (GameObject)GameObject.Instantiate(player, new Vector3(0,0,0), Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
        else
        {
            player2.GetComponent<PlayerObjsScript>().mychar = char2;
            player2 = (GameObject)GameObject.Instantiate(player2, new Vector3(0, 0, 0), Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player2, playerControllerId);
        }
        
        spawncount++;
    }

    public void DisconnectCall()
    {        
        NetworkManager.singleton.StopHost();
        MasterServer.UnregisterHost();
    }

    public void SwapChars(bool nchar1,bool nchar2)
    {
        if(nchar1)
        {
            char1 = explorer;
        }
        else
        {
            char1 = bird;
        }

        if (nchar2)
        {
            char2 = explorer;
        }
        else
        {
            char2 = bird;
        }
    }

    public void ChangeScene(string name)
    {
        ServerChangeScene(name);
    }

    public void CustomStartHost()
    {
        StartHost();
    }

    public void CustomStartClient(string hostip)
    {
        networkAddress = hostip;
        StartClient();
    }
}
