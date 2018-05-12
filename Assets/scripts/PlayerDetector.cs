using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool detected;
    // Use this for initialization
    void Start()
    {
        detected = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Expmove>())
            {
                if (other.gameObject.GetComponent<Expmove>().dead)
                {
                    detected = false;
                }
                else
                {
                    detected = true;
                }
            }
            else if (other.gameObject.GetComponent<Birdmove>())
            {
                if (other.gameObject.GetComponent<Birdmove>().dead)
                {
                    detected = false;
                }
                else
                {
                    detected = true;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            detected = false;
    }
}
