using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookDetect : MonoBehaviour {
    RaycastHit hit;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public GameObject EnemyRaycast()
    {
        bool isenemy = false;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
            if(hit.rigidbody.gameObject.tag == "Player")
            {
                isenemy = true;
            }
        }

        if (isenemy)
            return hit.transform.gameObject;
        else
            return default(GameObject);
    }
}
