using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RWGameManager;

public class EnemyController : MonoBehaviour {
    public Animator animator;
    public float movespeed = 5.0f;
    public int level = 1;
    public float recoveryTime = 2.0f;
    private CharacterController charcont;
    public GameObject target = null;
    private bool enemySpotted = false;
    private EnemyLookDetect lookDetect;
    private bool enemyAttackable = false;
    public float baseHealth = 100.0f;
    public float health = 0.0f;
    public float maxhealth;
    public Material EnemyMat;
    public Canvas EnemyUI;
    public GameObject ScoreText = default(GameObject);
    public GameManager gameManager;
    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        lookDetect = GetComponentInChildren<EnemyLookDetect>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        level = gameManager.round;
        health = Mathf.Round(baseHealth + (baseHealth * (level / 10)));
        maxhealth = health;
    }

    // Update is called once per frame
    void Update() {
        if (enemySpotted)
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
        
        EnemyUI.gameObject.transform.LookAt(new Vector3(EnemyUI.gameObject.transform.position.x, EnemyUI.gameObject.transform.position.y, EnemyUI.gameObject.transform.position.z + 1), Vector3.up);
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

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
        animator.enabled = false;
        EnemyUI.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(4.0f);
        Die();
    }

    public void Die()
    {
        EnemySpawner enspawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemySpawner>();
        gameManager.AddRoundWon(level);
        enspawn.enemyAlive = false;
        StopCoroutine(ScoreTextFade(default(GameObject)));
        Destroy(gameObject);
        return;
    }

    public void Damage(float dmg, ContactPoint point, Vector3 force, float swordspeed = 1)
    {
        if (health > dmg + (dmg + (force.magnitude * (swordspeed / 100))))
        {
            dmg = dmg + (force.magnitude * (swordspeed / 100));
            
            gameManager.AddToScore(dmg);

            health = health - (dmg + (force.magnitude * (swordspeed / 100)));

            GameObject ScoreTextSP = GameObject.FindGameObjectWithTag("ScoreTextSpawn");

            GameObject ScrText;

            if (ScoreTextSP.GetComponentInChildren<Text>())
            {
                ScrText = ScoreTextSP.GetComponentInChildren<Text>().gameObject;
                StopCoroutine(ScoreTextFade(default(GameObject)));
                ScrText.transform.localScale = ScoreText.transform.localScale;
            }
            else
            {
                ScrText = Instantiate(ScoreText, ScoreTextSP.transform);
            }
                
            
            ScoreTextSP.GetComponentInChildren<Text>().text = "- " + Mathf.Round(dmg);
            

            StartCoroutine(ScoreTextFade(ScrText));

            Rigidbody rb = GetComponentInChildren<Rigidbody>();
            Transform pos = GetComponentInChildren<Transform>();
            if (dmg > 50.0f)
            {
                animator.enabled = false;
                StartCoroutine(Recover());
            }

            rb.AddForceAtPosition(force * dmg, point.point);

        }
        else
            StartCoroutine(Finisher());

    }

    public void GetAttacked()
    {

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
        EnemyUI.GetComponentInChildren<Text>().text = Mathf.Round(health) + "/" + Mathf.Round(maxhealth);

        if ((health / maxhealth) * 100 < 15)
            EnemyUI.GetComponentInChildren<Text>().color = Color.red;

        Image enHealthImg = EnemyUI.GetComponentInChildren<Image>();
        enHealthImg.rectTransform.sizeDelta = new Vector2((health / maxhealth) * 100, enHealthImg.rectTransform.sizeDelta.y);
        EnemyUI.gameObject.transform.position = new Vector3(transform.position.x, EnemyUI.transform.position.y, transform.position.z);
    }
    
}
