using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NMOverwriter : NetworkManager
{
    public GameObject explorer, bird, playerobj;
    public GameObject[] spawners;
    int spawncount = 0;

    


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = playerobj;
        if (spawncount % 2 == 0)
        {
            player.GetComponent<PlayerObjsScript>().mychar = explorer;
            player = (GameObject)GameObject.Instantiate(player, spawners[0].transform.position, Quaternion.identity);
        }
        else
        {
            player.GetComponent<PlayerObjsScript>().mychar = bird;
            player = (GameObject)GameObject.Instantiate(player, spawners[1].transform.position, Quaternion.identity);
        }
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        spawncount++;
    }
}
