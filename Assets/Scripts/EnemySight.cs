using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {
    private EnemyController mainScript;
    private bool looking = true;
    private Quaternion newtrans;
    private bool rotationComplete;
    private bool turning = false;
    private GameObject target;
    // Use this for initialization
    void Start()
    {
        mainScript = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Searchforplayer();
        }
        else
        {
            mainScript.animator.SetFloat("X", 0);
            Vector3 targetloc = new Vector3(target.transform.position.x, -0.4f, -0.29f);
            mainScript.transform.LookAt(targetloc);
        }
    }

    void Searchforplayer()
    {
        if(looking)
        {
            if ((transform.rotation.y < 0)&&(!turning))
            {
                mainScript.animator.SetFloat("X", 1);
                turning = true;
            }
            else
            {
                mainScript.animator.SetFloat("X", -1);
                turning = true;
            }
                

            if ((transform.rotation.y >= 80)||(transform.rotation.y <= -80))
            {
                mainScript.animator.SetFloat("X", 0);
                turning = false;
            }
        }
        else
        {
            mainScript.animator.SetFloat("X", 0.0f);
        }
    }

    void OnTriggerEnter(Collider collided)
    {
        if (collided.tag == "Player")
        {
            target = collided.gameObject;
            looking = false;
            mainScript.target = collided.gameObject;
            mainScript.EnemyDetected(true);
        }
        
    }
    
}
