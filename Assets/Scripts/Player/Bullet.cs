using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private GameObject player;
    private GameObject wep;
    private JoystickController joyCon;
    private GameObject bulSpawn;
    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player");
        joyCon = player.GetComponent<JoystickController>();
        wep = joyCon.weapon;
        bulSpawn = GameObject.Find(wep.name + "BulletSpawn");
        this.gameObject.transform.localPosition = new Vector3(bulSpawn.transform.position.x, bulSpawn.transform.position.y, 0.0f);
        this.gameObject.transform.localRotation = new Quaternion(bulSpawn.transform.rotation.x, 0.0f, bulSpawn.transform.rotation.z * joyCon.looking, transform.rotation.w);
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(10.0f * joyCon.rightJoystickInput.x, joyCon.rightJoystickInput.y * 7.0f, 0);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().Damage(1, collision.contacts[0], collision, 7.0f);
        }

        StartCoroutine(Killer());
    }

    IEnumerator Killer()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        Destroy(gameObject);
    }
}
