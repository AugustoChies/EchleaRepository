﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwapNetwork : NetworkBehaviour {

    [SyncVar]
    public bool clicked;
    public GameObject unclicker;
    public GameObject mypobject;


    public void UpdateClicked(bool value)
    {
        mypobject.GetComponent<PlayerObjsScript>().UpdateClicked(this.gameObject, value);
    }

    public void UpdateCharClicked(bool value)
    {
        mypobject.GetComponent<PlayerObjsScript>().UpdateCharClicked(this.gameObject);
    }
}