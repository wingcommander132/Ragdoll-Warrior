using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class JoystickController : MonoBehaviour {
    public LeftJoystick leftJoystick;
    public RightJoystick rightJoystick;
    public Vector3 leftJoystickInput;
    public Vector3 rightJoystickInput;
    public int looking = 2;
    public GameObject weapon;
    public float swingspeed;
    public float time = 0;
    private bool wepactive = true;
    private Sword wep;
    private Vector3 playerRot;
    private Animator PlayerAnimator;
    private GameObject ArmMover;
    private GameObject weppos;
    private GameObject PHand;
    private AimIK playerarm;
    public bool hit = false;
    public bool SpeedGetRunning = false;
    // Use this for initialization
    void Start () {
        PlayerAnimator = GetComponent<Animator>();
        weppos = GameObject.FindGameObjectWithTag("WepActiveLoc");
        GameObject wepGO = GameObject.FindGameObjectWithTag("PlayerWeapon");
        wep = wepGO.GetComponent<Sword>();
        weapon = GameObject.FindGameObjectWithTag("PlayerWeapon");
        ArmMover = GameObject.FindGameObjectWithTag("ArmMover");
        PHand = ArmMover.GetComponent<ArmMover>().playerHand;
        playerarm = GetComponent<AimIK>();
    }
	
	// Update is called once per frame
	void Update () {
        playerRot = new Vector3 (transform.rotation.x, transform.rotation.y, transform.rotation.z);

        leftJoystickInput = leftJoystick.GetInputDirection();
        rightJoystickInput = rightJoystick.GetInputDirection();

        if (rightJoystickInput != Vector3.zero)
        {
            playerarm.enabled = true;
            ArmMover.transform.position = new Vector3(PHand.transform.position.x + rightJoystickInput.x, PHand.transform.position.y + rightJoystickInput.y);
            
            if (wepactive == false)
            {
                weapon.SetActive(true);
                wepactive = true;
            }

            weapon.transform.position = weppos.transform.position;

            if (SpeedGetRunning == false)
                 StartCoroutine(SpeedandDistance());
        }
        else
        {
            playerarm.enabled = false;
            ArmMover.transform.position = new Vector3(PHand.transform.position.x + rightJoystickInput.x, PHand.transform.position.y + rightJoystickInput.y);
            
            if (wepactive == true)
            {
                weapon.SetActive(false);
                wepactive = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (wepactive)
        {
            weapon.transform.position = weppos.transform.position;
            weapon.transform.LookAt(ArmMover.gameObject.transform.position);
        }
    }

    public void ReturnWepSpeed()
    {
        StartCoroutine(SpeedandDistance());
    }

    IEnumerator SpeedandDistance()
    {
        if(wepactive)
        {
            SpeedGetRunning = true;

            //Returns Units/Second
            Vector3 posA = weapon.transform.position;
            yield return new WaitForSecondsRealtime(0.01f);
            Vector3 posB = weapon.transform.position;

            swingspeed = Vector3.Distance(posA,posB) * 100;
            SpeedGetRunning = false;
        }
    }
}
