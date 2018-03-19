using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RWGameManager;
using RootMotion.FinalIK;

public class EnemyController : MonoBehaviour {
    public Animator animator;
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

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        GameObject obj = transform.parent.gameObject;
        parent = gameObject.transform.parent.gameObject;
        lowwarning = obj.GetComponentInChildren<LifeWarning>().gameObject;
        lowwarning.SetActive(false);

        Collider[] cols = parent.GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "EnemyBlocker")
                PlayerBlocker = col;
        }
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

    }

    public IEnumerator Recover()
    {
        PlayerBlocker.enabled = false;

        recovering = true;
        reacting = true;
        yield return new WaitForSeconds(recoveryTime);
        Vector3 save = transform.position;
        parent.transform.position = transform.position;
        animator.enabled = true;
        transform.position = save;
        animator.Play("GetUp");
        float wait = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSecondsRealtime(wait);

        PlayerBlocker.enabled = true;
        recovering = false;
        reacting = false;
        GetComponent<Collider>().enabled = true;
    }

    IEnumerator Attack()
    {

        attacking = true;

        animator.SetTrigger("Attack"); 

        yield return new WaitForSecondsRealtime(2.0f);

        attacking = false;
    }

    public IEnumerator Finisher()
    {
        PlayerBlocker.enabled = false;
        animator.enabled = false;
        yield return new WaitForSecondsRealtime(4.0f);
        Die();
    }

    public void Die()
    {
        EnemySpawner enspawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawner>();
        enspawn.enemyKilled();
        Destroy(gameObject.transform.parent.gameObject);
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
            if (Random.Range(0.0f, 10.0f) > 5.0f)
            {
                reacting = true;

                if (rb == default(Rigidbody))
                    print("EnemyPelvis tag likely not set.");

                animator.enabled = false;

                rb.AddForceAtPosition(force.relativeVelocity, new Vector3(0.0f, point.point.y, point.point.x));

                GetComponent<Collider>().enabled = false;

                StartCoroutine(Recover());

            }
            else
            {
                StartCoroutine(takeHit());
            }

        }
            

    }

    IEnumerator takeHit()
    {
        GetComponent<Collider>().enabled = false;
        reacting = true;
        animator.SetTrigger("Hit");
        yield return new WaitForSecondsRealtime(0.4f);
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
        if ((transform.localPosition.y < 0.14f)&&(!reacting))
            transform.localPosition = new Vector3(transform.localPosition.x, 0.16f, transform.localPosition.z);
        
    }
    
}
