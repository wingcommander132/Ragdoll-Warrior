using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    private GameObject activeLoc;
    private GameObject player;
    public float baseDamage = 10.0f;
    private bool isHit = false;
    private Collider swordcol;
    private ParticleSystem particles;
    // Use this for initialization
    void Start() {
        if(activeLoc == null)
            activeLoc = GameObject.FindGameObjectWithTag("WepActiveLoc");

        player = GameObject.FindGameObjectWithTag("Player");
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {

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

                if (player.GetComponent<JoystickController>().looking == 1)
                {
                    BounceEnd = new Vector3(ArmMover.transform.position.x - 2.0f, ArmMover.transform.position.y  + 1.5f, ArmMover.transform.position.z);
                }
                else
                {
                    BounceEnd = new Vector3(ArmMover.transform.position.x + 2.0f, ArmMover.transform.position.y + 1.5f, ArmMover.transform.position.z);
                }
            

                Vector3 diff = (ArmMover.transform.position - BounceEnd) / 2;

                Vector3[] test = new Vector3[4];

                test[0] = ArmMover.transform.position;
                test[1] = ArmMover.transform.position - diff;
                test[2] = test[1] - diff;
                test[3] = BounceEnd;

                LTBezierPath BounceBackPath = new LTBezierPath(test);
                LeanTween.move(ArmMover, BounceBackPath, 0.5f).setEase(LeanTweenType.easeOutQuad);
                particles.Play();
                float t = 0.5f;
                StartCoroutine(WaitAfterHit(t));
            }   
            
        }
    }

    IEnumerator WaitAfterHit(float time)
    {
        float scale = Time.timeScale;
        Time.timeScale = 0.6f;
        Handheld.Vibrate();
        yield return new WaitForSecondsRealtime(1.0f);
        Time.timeScale = scale;
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
