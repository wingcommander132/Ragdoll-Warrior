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
    private GameObject parent;
    public int turndirect = 0;
    private Vector3 goalRot;
    private bool turndone = true;
    private Collider turnstopper;
    // Use this for initialization
    void Start()
    {
        mainScript = GetComponentInParent<EnemyController>();
        parent = mainScript.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }
    
    void FixedUpdate()
    {
        if (target == null)
        {
            Searchforplayer();
        }
        else
        {
            StopCoroutine(PlayerSearch());

            mainScript.animator.SetFloat("X", 0);
            Vector3 targetloc = new Vector3(target.transform.position.x, mainScript.transform.position.y, 0.0f);

            Vector3 Veloc = Vector3.zero;
            mainScript.transform.LookAt(Vector3.SmoothDamp(mainScript.gameObject.transform.position, targetloc, ref Veloc, 0.5f));
            
        }
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

    void TurnCompleted()
    {
        turndone = true;
    }
    
    IEnumerator PlayerSearch()
    {
        if(!turning)
        {
            if (transform.rotation.eulerAngles.y < 100)
            {
                mainScript.animator.SetFloat("X", 1);
                turning = true;
                turndirect = -1;
                goalRot = new Vector3(transform.rotation.eulerAngles.x, 270.0f, transform.rotation.eulerAngles.z);
            }
            else
            {
                mainScript.animator.SetFloat("X", -1);
                turning = true;
                turndirect = 1;
                goalRot = new Vector3(transform.rotation.eulerAngles.x, 90.0f, transform.rotation.eulerAngles.z);
            }
            turndone = false;

            
        }

        LTDescr lean = LeanTween.rotate(parent, goalRot, 2.0f);
        lean.setOnComplete(TurnCompleted);
        yield return new WaitUntil(() => turndone == true);

        mainScript.animator.SetFloat("X", 0, 0.04f, Time.deltaTime);

        yield return new WaitForSecondsRealtime(4.0f);

        turning = false;
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
