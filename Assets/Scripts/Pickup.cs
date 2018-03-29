using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public string itemName;
    public int itemType;
    public GameObject pickupModel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 1, 0));
	}

    public void itemCollected()
    {
        Destroy(gameObject);
    }
}
