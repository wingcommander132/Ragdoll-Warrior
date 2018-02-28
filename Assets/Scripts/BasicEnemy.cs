using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour {
    public float health = 100.0f;
    private Quaternion oldrotation;
    public Material EnemyMat;
    public Canvas EnemyUI;
    public GameObject Parent;
    // Use this for initialization
    void Start () {
        Parent = transform.parent.gameObject;
	}
	
    void FixedUpdate()
    {
        EnemyUI.GetComponentInChildren<Text>().text = Mathf.Round(health).ToString();
        Image enHealthImg = EnemyUI.GetComponentInChildren<Image>();
        enHealthImg.rectTransform.sizeDelta = new Vector2(health, enHealthImg.rectTransform.sizeDelta.y);
        EnemyUI.gameObject.transform.position = new Vector3(transform.position.x, EnemyUI.transform.position.y, transform.position.z);
    }

    public void Die()
    {
        EnemySpawner enspawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawner>();
        enspawn.enemyAlive = false;
        Destroy(transform.parent.gameObject);
        return;
    }

	// Update is called once per frame
	void Update () {
        if((transform.rotation.z != 0)&&(gameObject.activeSelf == true))
        {
            StartCoroutine(ReUp(new Quaternion(0,0,0,0)));
        }

        
        
	}

    public void Damage(float dmg, ContactPoint point, Vector3 force)
    {
        if (health >= dmg)
        {
            float totalforce = (force.x + force.y + force.z) / 3;
            health = health - dmg;
            oldrotation = transform.rotation;
            Rigidbody rb = GetComponent<Rigidbody>();
            Transform pos = GetComponentInChildren<Transform>();

            rb.AddForceAtPosition(force, point.point);
            StartCoroutine(Recover(oldrotation));

        }
        else
            Die();

    }

    IEnumerator Recover(Quaternion rotation)
    {
        yield return new WaitForSeconds(0.1f);
        transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, transform.position.z), rotation);
    }

    IEnumerator ReUp(Quaternion rotation)
    {
        Quaternion oldrotate = transform.rotation;
        yield return new WaitForSeconds(1.0f);
        Quaternion testedrotate = transform.rotation;

        if(oldrotate == testedrotate)
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, transform.position.z), rotation);
        }
    }
}
