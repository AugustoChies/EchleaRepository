using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwapNetwork : NetworkBehaviour {

    [SyncVar]
    public bool clicked;
    public GameObject unclicker;
    public GameObject mypobject;
    public bool switchchar;

    public void UpdateClicked(bool value)
    {
        mypobject.GetComponent<PlayerObjsScript>().UpdateClicked(this.gameObject, value);
    }

    public void UpdateCharClicked(bool value)
    {
        mypobject.GetComponent<PlayerObjsScript>().UpdateCharClicked(this.gameObject);
    }

    public void Swap()
    {
        mypobject.GetComponent<PlayerObjsScript>().SwapChar(this.gameObject);
    }
}
