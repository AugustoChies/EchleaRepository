using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class digsitescript : NetworkBehaviour
{
    public int type = 0; //0 nada, 1 esqueletos, 2 chave, 3 relíquia
    public int relindex = 0; //só se tem reliquia,0 padrão, de 1 a 9 para as reliquias coletaveis.
    [SyncVar]
    public bool dug;
    bool changed;
    float scannetimer;
    public GameObject inventory;
    public GameObject icon;
    public GameObject bird;
    public GameObject explorer;
    
    // Use this for initialization
    void Start()
    {
        dug = false;
        scannetimer = -1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(explorer == null)
        {            
            explorer = GameObject.Find("Explorer(Clone)");
        }

        if (bird == null)
        {
            bird = GameObject.Find("Bird(Clone)");
        }

        if (scannetimer >= 0)
        {
            scannetimer += 1 * Time.deltaTime;
            if (scannetimer > 3)
            {                
                scannetimer = -1;
            }
        }
        if(dug && !changed)
        {
            changed = true;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1.0f, 0);
        }
    }

    

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "ataqueex")
        {
            if (!dug)
            {
                    
                    dug = true;
                    Dug();
                
                
            }
        }
        
        if (other.gameObject.tag == "ataquebi")
        {
            if (!dug && !bird.GetComponent<Birdmove>().carrying)
            {
                if (type == 0 || type == 1)
                {
                    dug = true;
                    Dug();
                }
                else
                {
                    
                    dug = true;
                    BirdDug();
                    
                }
                

            }
        }

        if (other.gameObject.tag == "scanex")
        {
            if (!dug && scannetimer == -1)
            {
                ShowIcon();
                scannetimer = 0;
            }
        }
        if (other.gameObject.tag == "scanbi")
        {
            if (!dug && scannetimer == -1)
            {
                ShowIcon();
                scannetimer = 0;
            }
        }

    }

    void Dug()
    {
        if (type == 1)
        {
            inventory.GetComponent<Inventoryscr>().sendInfo(-2);
        }
        else if (type == 0)
        {
            inventory.GetComponent<Inventoryscr>().sendInfo(-1);
        }
        else if (type == 2)
        {
            inventory.GetComponent<Inventoryscr>().sendInfo(0);
        }
        else if(type == 3)
        {
            inventory.GetComponent<Inventoryscr>().sendInfo(relindex);
        }

        if (explorer != null)
            explorer.GetComponent<Expmove>().Dig(this.gameObject);
        if (bird != null)
            bird.GetComponent<Birdmove>().Dig(this.gameObject);
    }

    void BirdDug()
    {
        
        if (type == 1)
        {
            inventory.GetComponent<Inventoryscr>().sendInfo(-2);
        }
        else if (type == 0)
        {
            inventory.GetComponent<Inventoryscr>().sendInfo(-1);
        }
        else
        { 
            if(type == 3)
                bird.GetComponent<Birdmove>().Changecarrying(true,relindex);
            else
                bird.GetComponent<Birdmove>().Changecarrying(true, 0);
        }
        
        if (explorer != null)
            explorer.GetComponent<Expmove>().Dig(this.gameObject);
        if (bird != null)
            bird.GetComponent<Birdmove>().Dig(this.gameObject);
    }

    void ShowIcon()
    {
        if (explorer != null)
            explorer.GetComponent<Expmove>().ShowIcon(this.gameObject);
        if (bird != null)
            bird.GetComponent<Birdmove>().ShowIcon(this.gameObject);
    }
}
