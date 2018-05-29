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

    public void SwapChars()
    {
        if(char1.GetComponent<Expmove>())
        {
            char1 = bird;
        }
        else
        {
            char1 = explorer;
        }

        if (char2.GetComponent<Expmove>())
        {
            char2 = bird;
        }
        else
        {
            char2 = explorer;
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
