using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingCollider : MonoBehaviour {
    private Vector3 BlockerPos;
    private Quaternion BlockerRot = new Quaternion(0.0f,90.0f,0.0f,0.0f);
    // Use this for initialization
    void Start () {
        BlockerRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = this.transform.parent.gameObject.GetComponentInChildren<EnemyController>().transform.position;
        transform.rotation = BlockerRot;
    }
}
