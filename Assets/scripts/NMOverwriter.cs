using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NMOverwriter : NetworkManager
{
    public GameObject explorer, bird, playerobj;
    public GameObject[] spawners;
    GameObject player;
    GameObject player2;
    int spawncount = 0;

    


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        player = playerobj;
        player2 = playerobj;
        if (spawncount % 2 == 0)
        {
            player.GetComponent<PlayerObjsScript>().mychar = explorer;
            player = (GameObject)GameObject.Instantiate(player, spawners[0].transform.position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
        else
        {
            player2.GetComponent<PlayerObjsScript>().mychar = bird;
            player2 = (GameObject)GameObject.Instantiate(player, spawners[1].transform.position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player2, playerControllerId);
        }
        
        spawncount++;
    }
}
