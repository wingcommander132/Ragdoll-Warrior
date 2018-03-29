using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RWGameManager;
using UnityEngine.UI;

public class MobileRuntimeSetup : MonoBehaviour {
    private GameManager manager;
    private bool hasManager = false;
	// Use this for initialization
	void Start () {
        if(GameObject.FindGameObjectWithTag("GameController"))
        {
            hasManager = true;
            manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }

        Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnApplicationFocus(bool focus)
    {
        if(focus == false && !Application.isEditor)
        {
            Application.runInBackground = true;

            if(hasManager && !manager.pauseUI.activeSelf)
                manager.PauseGame();
            
        }
    }
}
