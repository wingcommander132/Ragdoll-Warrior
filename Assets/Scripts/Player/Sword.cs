using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    private GameObject activeLoc;
    private GameObject player;
    public float baseDamage = 10.0f;
    private bool isHit = false;
    public GameObject mainBody;
    private Collider swordcol;
    // Use this for initialization
    void Start() {
        if(activeLoc == null)
            activeLoc = GameObject.FindGameObjectWithTag("WepActiveLoc");

        player = GameObject.FindGameObjectWithTag("Player");

        //position = activeLoc.transform.position;
        //transform.rotation = activeLoc.transform.rotation;
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
            if (!isHit)
            {
                (swordcol = GetComponent<MeshCollider>()).enabled = false;
                EnemyController enemy = en.GetComponent<EnemyController>();
                ContactPoint point = colis.contacts[0];
                player.GetComponent<JoystickController>().ReturnWepSpeed();

                if (!isHit)
                    enemy.Damage(baseDamage, point, colis);

                isHit = player.GetComponent<JoystickController>().hit = true;
                GameObject ArmMover = GameObject.FindGameObjectWithTag("ArmMover");

                float swingspd = player.GetComponent<JoystickController>().swingspeed;

                Vector3 BounceEnd = Vector3.zero;

                if (player.GetComponent<JoystickController>().looking == 2)
                {
                    BounceEnd = new Vector3((-ArmMover.transform.position.x + ((swingspd / 300) + colis.relativeVelocity.normalized.magnitude)) / 2, ((ArmMover.transform.position.y + (swingspd / 300)) + colis.relativeVelocity.normalized.magnitude), ArmMover.transform.position.z).normalized * -10;
                    BounceEnd = new Vector3(BounceEnd.x, -BounceEnd.y, BounceEnd.z);
                }
                else
                {
                    BounceEnd = new Vector3(-(ArmMover.transform.position.x - ((swingspd / 300) + colis.relativeVelocity.normalized.magnitude)) / 2, ((ArmMover.transform.position.y + (swingspd / 300)) + colis.relativeVelocity.normalized.magnitude) / 2, ArmMover.transform.position.z).normalized * 10;
                }
            

                Vector3 diff = (ArmMover.transform.position - BounceEnd) / 2;

                Vector3[] test = new Vector3[4];

                test[0] = ArmMover.transform.position;
                test[1] = ArmMover.transform.position - diff;
                test[2] = test[1] - diff;
                test[3] = BounceEnd;

                LTBezierPath BounceBackPath = new LTBezierPath(test);
                LeanTween.move(ArmMover, BounceBackPath, 0.4f * colis.relativeVelocity.normalized.magnitude).setEase(LeanTweenType.easeOutQuad);

                float t = 0.4f * colis.relativeVelocity.normalized.magnitude;
                StartCoroutine(WaitAfterHit(t));
            }   
            
        }
    }

    IEnumerator WaitAfterHit(float time)
    {
        yield return new WaitForSeconds(time);
        swordcol.enabled = true;
        isHit = player.GetComponent<JoystickController>().hit = false;
    }

    void OnCollisionExit(Collision colis)
    {
        GameObject en = colis.gameObject;
        if (en.tag == "Enemy")
        {
            swordcol.enabled = true;
            EnemyController enemy = en.GetComponent<EnemyController>();
            isHit = player.GetComponent<JoystickController>().hit = false;
        }
    }

}
