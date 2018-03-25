using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RWGameManager;
using RootMotion.FinalIK;

public class EnemyController : MonoBehaviour {
    public Animator animator;
    public float forcedZPos;
    public float recoveryTime = 2.0f;
    public GameObject target = null;
    private bool enemySpotted = false;
    private bool enemyAttackable = false;
    public int maxhealth = 2;
    //public GameManager gameManager;
    private bool reacting = false;
    public int lookdirect = 0;
    private bool attacking = false;
    private GameObject lowwarning;
    public bool recovering = false;
    public GameObject parent;
    private int hitsTaken = 0;
    private Vector3 takenColis = Vector3.zero;
    private Collider PlayerBlocker;
    public string blockAnimationString;
    public string[] attackAnimationStrings;
    public string[] secondaryAnimationStrings;
    public GameObject wep;
    private MeshCollider wepCol;
    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        lowwarning = GetComponentInChildren<LifeWarning>().gameObject;
        lowwarning.SetActive(false);
        wepCol = wep.GetComponent<MeshCollider>();
        wepCol.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        if (maxhealth - hitsTaken <= 1)
            lowwarning.SetActive(true);
        
        lookdirect = GetComponentInChildren<EnemySight>().turndirect;
        if (!reacting)
        {
            if (enemySpotted)
            {
                if ((enemyAttackable) && (!attacking))
                {
                    animator.SetFloat("Y", 0.0f, 0.1f, Time.deltaTime);
                    StartCoroutine(Attack());
                }
                else
                if(attacking)
                {
                    animator.SetFloat("Y", 0.0f, 0.1f, Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("Y", 1.6f, 0.2f, Time.deltaTime);
                    attacking = false;
                    StopCoroutine(Attack());
                }
                               
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

        }
        
    }

    public void GamePaused(bool pause)
    {
        if(pause)
        {
            reacting = true;
            animator.enabled = false;
        }
        else
        {
            reacting = false;
            animator.enabled = true;
            animator.Play("GetUp");
        }
    }

    void OnCollisionEnter(Collision cols)
    {
        if(cols.gameObject.tag == "Killer")
        {
            StartCoroutine(Finisher());
        }
    }

    public IEnumerator Recover()
    {
        //GetComponent<Collider>().enabled = false;
        recovering = true;
        reacting = true;
        yield return new WaitForSeconds(recoveryTime);
        animator.enabled = true;
        animator.Play("GetUp");
        float wait = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSecondsRealtime(wait);
        
        recovering = false;
        reacting = false;
        GetComponent<Collider>().enabled = true;
    }

    IEnumerator Attack()
    {
        wepCol.enabled = true;
        attacking = true;

        animator.Play(Animator.StringToHash(attackAnimationStrings[Random.Range(0, attackAnimationStrings.Length - 1)]));
        float wait = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSecondsRealtime(3.0f);
        transform.LookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));

        attacking = false;
        wepCol.enabled = false;
    }

    public IEnumerator Finisher()
    {
        animator.enabled = false;
        yield return new WaitForSecondsRealtime(4.0f);
        Die();
    }

    public void Die()
    {
        EnemySpawner enspawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawner>();
        enspawn.enemyKilled();
        Destroy(gameObject);
        return;
    }

    public void Damage(float dmg, ContactPoint point, Collision force = null, float swordspeed = 1)
    {
        hitsTaken++;
        if(hitsTaken >= maxhealth)
        {
            GetComponent<Collider>().enabled = false;
            EnemySpawner enspawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawner>();
            StartCoroutine(Finisher());
            return;
        }

        Rigidbody rb = default(Rigidbody);
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        int cnt = 0;
        foreach (Rigidbody ridg in rbs)
        {
            cnt++;
            if (ridg.tag == "EnemyPelvis")
                rb = ridg;
        }

        takenColis = rb.velocity;

        float stutter = force.relativeVelocity.magnitude - takenColis.magnitude;
        if (!reacting)
        {
             StartCoroutine(takeHit());
        }
            

    }

    IEnumerator takeHit()
    {
        GetComponent<Collider>().enabled = false;
        reacting = true;
        animator.Play(Animator.StringToHash(secondaryAnimationStrings[Random.Range(0,secondaryAnimationStrings.Length-1)]));
        yield return new WaitForSecondsRealtime(0.5f);
        GetComponent<Collider>().enabled = true;
        reacting = false;
    }

    IEnumerator ScoreTextFade(GameObject Scrtext)
    {
        Vector3 currentVelocity = Vector3.zero;
        Scrtext.transform.position = Vector3.SmoothDamp(Scrtext.transform.position, Scrtext.transform.position * 3, ref currentVelocity, 3.0f);
        yield return new WaitForSeconds(2.0f);
        Vector3 currentVelocity2 = Vector3.zero;
        Scrtext.transform.localScale = Vector3.SmoothDamp(Scrtext.transform.localScale, Vector3.zero, ref currentVelocity2, 1.0f);
        yield return new WaitForSeconds(1.0f);
        Scrtext.SetActive(false);
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
        if (transform.position.z != forcedZPos)
            transform.position = new Vector3(transform.position.x, transform.position.y, forcedZPos);
        


    }
    
}
