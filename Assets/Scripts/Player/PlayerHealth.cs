using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public int health = 5;
    public int maxHealth;
    public Text healthBar;
    public bool recovering = false;
	// Use this for initialization
	void Start () {
        maxHealth = health;
        healthBar = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        healthBar.text = health.ToString();

        if(recovering == true)
        {
            StartCoroutine(Recover());
        }

        if (health <= 0)
        {
            Die();
        }

        if(GetComponent<CharacterController>().enabled == true)
        {
            GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }

        
	}

    IEnumerator Recover()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        recovering = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "EnemyWeapon")
            {
                if(!recovering)
                {
                    recovering = true;
                    health = health - 1;
                }
                    
            }

            if (col.gameObject.tag == "Killer")
            {
                Die();
            }
    }

    void Die()
    {
        SceneManager.LoadScene(0);

    }

    void Damage()
    {

    }
}
