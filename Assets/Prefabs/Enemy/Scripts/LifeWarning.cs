using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeWarning : MonoBehaviour {
    private GameObject Enemy;
    private Vector3 newrot = Vector3.zero;
    private EnemyController enScript;
    private bool hovering = false;
    private Light glow;
    private bool running = false;
    // Use this for initialization
    void Start () {
        glow = GetComponentInChildren<Light>();

        
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0.0f, 2.5f, 0.0f, Space.Self);
        
        if(!running)
            StartCoroutine(Glow());
    }

    IEnumerator Glow()
    {
        running = true;
        glow.range = 1.12f;
        yield return new WaitForSeconds(1.0f);
        glow.range = 0.0f;
        yield return new WaitForSeconds(1.0f);
        running = false;
    }

    void FixedUpdate()
    {

    }
    
}
