using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour {
    private GameObject activeLoc;
    private GameObject player;
    public int weaponIndex = 2;
    public float baseDamage = 10.0f;
    //private ParticleSystem particles;
    // Use this for initialization
    void Start()
    {
        if (activeLoc == null)
            activeLoc = GameObject.FindGameObjectWithTag("WepActiveLoc");

        player = GameObject.FindGameObjectWithTag("Player");
        //particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

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
