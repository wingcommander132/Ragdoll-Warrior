using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {
    private EnemyController mainScript;
    private bool looking = true;
    private Quaternion newtrans;
    private bool rotationComplete;
    public bool turning = false;
    private GameObject target = null;
    public int turndirect = 0;
    private Quaternion goalRot;
    private bool turndone = true;
    private Collider turnstopper;
    private GameObject LStop;
    private GameObject RStop;
    // Use this for initialization
    void Start()
    {
        mainScript = GetComponentInParent<EnemyController>();

        Collider[] cols = transform.parent.gameObject.transform.parent.gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "EnemyLLookStop")
                LStop = col.gameObject;

            if (col.gameObject.tag == "EnemyRLookStop")
                RStop = col.gameObject;
        }

        turnstopper = RStop.GetComponent<Collider>();
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
            Vector3 targetloc = new Vector3(target.transform.position.x, mainScript.transform.position.y, 0.0f);
            
            Vector3 Veloc = Vector3.zero;
            mainScript.transform.LookAt(Vector3.SmoothDamp(mainScript.gameObject.transform.position, targetloc,ref Veloc, 0.5f));
        }
    
    }
    
    void FixedUpdate()
    {
        LStop.transform.position = new Vector3(mainScript.transform.position.x - 4, 0.0f, 0.0f);
        RStop.transform.position = new Vector3(mainScript.transform.position.x + 4, 0.0f, 0.0f);
    }

    void Searchforplayer()
    {
        if (turndone == true)
            mainScript.animator.SetFloat("X", 0, 0.1f, Time.deltaTime);

        if ((looking)&&(!turning))
        {
            StartCoroutine(PlayerSearch());
        }
    }
    
    IEnumerator PlayerSearch()
    {
        if(!turning)
        {
            if (transform.rotation.y > 0)
            {
                mainScript.animator.SetFloat("X", 1);
                turning = true;
                turndirect = -1;
            }
            else
            {
                mainScript.animator.SetFloat("X", -1);
                turning = true;
                turndirect = 1;
            }
            turndone = false;

            goalRot = new Quaternion(transform.rotation.x, (transform.rotation.y + (80 * turndirect)), transform.rotation.z, transform.rotation.w);
        }
        yield return new WaitForSecondsRealtime(1.0f);

        turnstopper.enabled = true;
        
        yield return new WaitUntil(() => turndone == true);
        
        yield return new WaitForSecondsRealtime(3.0f);

        turning = false;
    }

    IEnumerator TestIfTurned()
    {
        
        if (transform.rotation.y*100 == goalRot.y)
            turning = false;
        else
            yield return new WaitForSeconds(0.5f);


        if (transform.rotation.y == goalRot.y)
            turning = false;
        else
            yield return StartCoroutine(TestIfTurned());

    }

    void OnTriggerEnter(Collider collided)
    {
        if ((collided.tag == "Player")&&(target == null))
        {
            target = collided.gameObject;
            looking = false;
            mainScript.target = collided.gameObject;
            mainScript.EnemyDetected(true);
        }

        if((collided.tag == "EnemyLLookStop")||(collided.tag == "EnemyRLookStop"))
        {
            turnstopper = collided;
            turnstopper.enabled = false;
            turndone = true;
            
            if (collided.tag == "EnemyLLookStop")
            {
                mainScript.gameObject.transform.LookAt(new Vector3(-100.0f, mainScript.gameObject.transform.position.y, 0.0f));
                turndirect = 1;
            }
                
            else
            {
                mainScript.gameObject.transform.LookAt(new Vector3(100.0f, mainScript.gameObject.transform.position.y, 0.0f));
                turndirect = -1;
            }
                
        }

    }
    
}
