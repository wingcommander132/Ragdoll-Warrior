using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour {
    private GameObject activeLoc;
    private GameObject player;
    public int weaponIndex = 1;
    public float baseDamage = 10.0f;
    public GameObject bulletSpawn;
    public GameObject bulletPrefab;
    private JoystickController joycon;
    public int fireRate = 1;
    public GameObject muzzleFlash;
    public bool equiped = false;
    //private ParticleSystem particles;
    // Use this for initialization
    void Start()
    {
        activeLoc = GameObject.FindGameObjectWithTag("WepActiveLoc");
        player = GameObject.Find("Player");
        joycon = player.GetComponent<JoystickController>();

        if (joycon.weapon == this.gameObject)
            StartCoroutine(TestFire());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        bulletSpawn.transform.LookAt(GameObject.FindGameObjectWithTag("ArmMover").transform.position, new Vector3(0, 0, 1));
    }

    private void OnEnable()
    {
        if (activeLoc == null)
            activeLoc = GameObject.FindGameObjectWithTag("WepActiveLoc");

        StartCoroutine(TestFire());
    }

    IEnumerator TestFire()
    {
        int shotCount = 0;
        while (shotCount != fireRate)
        {
            Fire();
            yield return new WaitForSecondsRealtime(1.0f / fireRate);
            shotCount++;
        }
        yield return StartCoroutine(TestFire());
    }

    IEnumerator muzFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);
    }

    void Fire()
    {
        StartCoroutine(muzFlash());
        GameObject bull = Instantiate(bulletPrefab);
    }
}
