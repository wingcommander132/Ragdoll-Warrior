using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {
    private EnemyController mainScript;
	// Use this for initialization
	void Start () {
        mainScript = GetComponentInParent<EnemyController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collided)
    {
        if (collided.tag == "Player")
        {
            mainScript.target = collided.gameObject;
            mainScript.EnemyDetected(true);
        }
    }
}
