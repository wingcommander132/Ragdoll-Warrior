using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPusher : MonoBehaviour {

    public GameObject Player;
    public float zoomDistance = 1.42f;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    private void FixedUpdate()
    {
        if ((this.transform.position.x != Player.transform.position.x) || (this.transform.position.y != Player.transform.position.y))
        {
            //Vector3 velocity = default(Vector3);
            Vector3 newpos = new Vector3(Player.transform.position.x, Player.transform.position.y, (Player.transform.position.z - zoomDistance));
            transform.position = newpos;
        }
    }

}
