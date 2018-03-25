using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTracker : MonoBehaviour {
    private Vector3 startpos;
    private Vector3 endpos;
    private GameObject Player;
    private JoystickController joycon;
    private float xdirect = 0.0f;
    private float ydirect = 0.0f;
    private int swingdirect = 0;
	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        joycon = Player.GetComponent<JoystickController>();
	}
	
    void OnEnable()
    {
        startpos = transform.position;
    }

	// Update is called once per frame
	void Update () {
        if (swingdirect == 0)
        {
            StartCoroutine(GetDirect());
        }

        if(swingdirect == -1)
        {
            StartCoroutine(GetDirect());

            if (swingdirect != -1)
            {
                startpos = transform.position;
                swingdirect = 0;
            }
        }
        else
        if (swingdirect == 1)
        {
            StartCoroutine(GetDirect());

            if (swingdirect != 1)
            {
                startpos = transform.position;
                swingdirect = 0;
            }
        }
        
    }

    IEnumerator GetDirect()
    {
        yield return new WaitForSeconds(1);

        if(transform.position == startpos)
        {
            swingdirect = 0;
        }
        else
        {
            float diffx = transform.position.x - startpos.x;
            float diffy = transform.position.y - startpos.y;
            float difflength = diffx + diffy;
            if (difflength < 0.0f)
                swingdirect = -1;
            else
            if (difflength > 0.0f)
                swingdirect = 1;
            else
                swingdirect = 0;
        }

        yield return swingdirect;
    }

    void OnTriggerEnter(Collider collided)
    {
        endpos = transform.position;
    }
    /*
    public float Weapon_hit()
    {
        StopCoroutine(Calc_MoveTime());
        float timeval = time;
        hit = true;
        time = 0;
        return timeval;
    }

    IEnumerator Calc_MoveTime()
    {
        yield return new WaitForSeconds(1);
        time = time + 1;
    }

    IEnumerator Calc_speed()
    {
        Vector3 startpoint = rightJoystickInput;
        yield return new WaitUntil(() => hit == true);
        Vector3 CurrentPoint = rightJoystickInput;
        if (CurrentPoint != startpoint)
        {
            swingspeed = (CurrentPoint.x - startpoint.x) + (CurrentPoint.y - startpoint.y);
            if (swingspeed < 0)
                swingspeed = swingspeed * -1;
        }
        else
        {
            swingspeed = 0.0f;
        }
    }
    */
}
