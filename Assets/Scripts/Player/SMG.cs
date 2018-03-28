using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : MonoBehaviour {

    private GameObject activeLoc;
    private GameObject player;
    public int weaponIndex = 1;
    public float baseDamage = 10.0f;
    public GameObject bulletSpawn;
    public GameObject bulletPrefab;
    //private ParticleSystem particles;
    // Use this for initialization
    void Start()
    {
        if (activeLoc == null)
            activeLoc = GameObject.FindGameObjectWithTag("WepActiveLoc");

        player = GameObject.FindGameObjectWithTag("Player");
        //particles = GetComponentInChildren<ParticleSystem>();

        StartCoroutine(TestFire());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        
    }

    private void OnEnable()
    {
        if (activeLoc == null)
            activeLoc = GameObject.FindGameObjectWithTag("WepActiveLoc");

        player = GameObject.FindGameObjectWithTag("Player");
        //particles = GetComponentInChildren<ParticleSystem>();

        StartCoroutine(TestFire());
    }

    IEnumerator TestFire()
    {
        print("fire");
        Fire();
        yield return new WaitForSecondsRealtime(1.0f);
        yield return StartCoroutine(TestFire());
    }

    void Fire()
    {
        //Transform bulltransform = transform;
        //bulltransform.rotation = new Quaternion(0.0f, 0.0f, bulltransform.rotation.z,0.0f);
        GameObject bull = Instantiate(bulletPrefab,bulletSpawn.transform);
        bull.transform.parent.DetachChildren();
        bull.transform.position = bulletSpawn.transform.position;
        bull.transform.rotation = bulletSpawn.transform.rotation;
        //bull.transform.position = new Vector3(bull.transform.position.x, bull.transform.position.y, 0.0f);
        bull.GetComponent<Rigidbody>().velocity = new Vector3(50, 0, 0);
    }

    IEnumerator WaitAfterHit(float time)
    {
        float scale = Time.timeScale;
        Time.timeScale = 0.6f;
        Handheld.Vibrate();
        yield return new WaitForSecondsRealtime(1.0f);
        Time.timeScale = scale;
        //swordcol.enabled = true;
        //isHit = player.GetComponent<JoystickController>().hit = false;
    }

    void OnCollisionExit(Collision colis)
    {/*
        GameObject en = colis.gameObject;
        if (en.tag == "Enemy")
        {
            swordcol.enabled = true;
            EnemyController enemy = en.GetComponent<EnemyController>();
            isHit = player.GetComponent<JoystickController>().hit = false;
        }*/
    }
}
