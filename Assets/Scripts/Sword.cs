using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    private GameObject activeLoc;
    private GameObject player;
    public float baseDamage = 10.0f;
    // Use this for initialization
    void Start() {
        if(activeLoc == null)
            activeLoc = GameObject.FindGameObjectWithTag("WepActiveLoc");

        player = GameObject.FindGameObjectWithTag("Player");

        transform.position = activeLoc.transform.position;
        transform.rotation = activeLoc.transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(activeLoc.transform.position.x, activeLoc.transform.position.y, transform.position.z);
    }

    void OnCollisionEnter(Collision colis)
    {
        GameObject en = colis.gameObject;
        
        if (en.tag == "Enemy")
        {
            EnemyController enemy = en.GetComponent<EnemyController>();
            ContactPoint point = colis.contacts[0];

            enemy.Damage(baseDamage, point, colis.impulse);
        }
    }

}
