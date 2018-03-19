using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackControll : MonoBehaviour {
    private EnemyController mainScript;
    // Use this for initialization
    void Start()
    {
        mainScript = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collided)
    {
        if (collided.tag == "Player")
        {
            GetComponentInParent<EnemyController>().EnemyInAttackRange(true);
            mainScript.animator.SetFloat("Y", 0.0f);
        }
    }

    void OnTriggerExit(Collider collided)
    {
        if (collided.tag == "Player")
        {
            mainScript.EnemyInAttackRange(false);
        }
    }

}
