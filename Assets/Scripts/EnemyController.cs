using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    public Animator animator;
    public float movespeed = 5.0f;
    public float recoveryTime = 2.0f;
    private CharacterController charcont;
    public GameObject target = null;
    private bool enemySpotted = false;
    private EnemyLookDetect lookDetect;
    private bool enemyAttackable = false;
    public float health = 100.0f;
    private float maxhealth;
    public Material EnemyMat;
    public Canvas EnemyUI;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        lookDetect = GetComponentInChildren<EnemyLookDetect>();
        maxhealth = health;
    }
	
	// Update is called once per frame
	void Update () {
        if(enemySpotted)
        {
            if (enemyAttackable)
            {
                animator.SetFloat("Y", 0.0f);
            }
            else
            {
                Vector3 moveLoc = Vector3.zero;
                if (transform.position.x - target.transform.position.x > 0)
                {
                    moveLoc = new Vector3(-movespeed, 0, 0);
                    
                }
                else
                {
                    moveLoc = new Vector3(movespeed, 0, 0);
                }
                
                animator.SetFloat("Y", 2.0f);
            }
            

        }

        transform.position = new Vector3(transform.position.x,transform.position.y,0.0f);

        if (health <= 0)
            StartCoroutine(Finisher());
    }

    void OnCollisionEnter(Collision cols)
    {

    }

    public IEnumerator Recover()
    {
        yield return new WaitForSeconds(recoveryTime);
        animator.enabled = true;
        animator.Play("GetUp");
    }

    public IEnumerator Finisher()
    {
        yield return new WaitForSeconds(recoveryTime);
        Die();
    }

    public void Die()
    {
        EnemySpawner enspawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawner>();
        enspawn.enemyAlive = false;
        Destroy(transform.parent.gameObject);
        return;
    }

    public void Damage(float dmg, ContactPoint point, Vector3 force, float swordspeed = 1)
    {
        if (health > dmg * swordspeed)
        {
            health = health - (dmg * swordspeed);
            Rigidbody rb = GetComponentInChildren<Rigidbody>();
            Transform pos = GetComponentInChildren<Transform>();
            
            if(swordspeed > 2)
            {
                animator.enabled = false;
                StartCoroutine(Recover());
            }
            
            rb.AddForceAtPosition(force, point.point);

        }
        else
            StartCoroutine(Finisher());

    }

    public void GetAttacked()
    {

    }

    public void EnemyDetected(bool detected)
    {
        enemySpotted = detected;
        if (detected)
        {
            
        }
        else
        {

        }
    }

    public void EnemyInAttackRange(bool inRange)
    {
        enemyAttackable = inRange;
        if(inRange)
        {

        }
        else
        {

        }
    }

    void FixedUpdate()
    {
        EnemyUI.GetComponentInChildren<Text>().text = Mathf.Round((health/maxhealth) * 100).ToString();
        Image enHealthImg = EnemyUI.GetComponentInChildren<Image>();
        enHealthImg.rectTransform.sizeDelta = new Vector2((health / maxhealth) * 100, enHealthImg.rectTransform.sizeDelta.y);
        EnemyUI.gameObject.transform.position = new Vector3(transform.position.x, EnemyUI.transform.position.y, transform.position.z);
    }
    
}
