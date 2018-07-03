using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSongScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!(SceneManager.GetActiveScene().name == "Main Menu" || SceneManager.GetActiveScene().name == "Playing" || SceneManager.GetActiveScene().name == "CharacterSelect"))
        {
            Destroy(this.gameObject);
        }
    }
}
