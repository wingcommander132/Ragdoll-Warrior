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
    private bool wepactive = false;
    private Sword wep;
    private Vector3 playerRot;
    private Animator PlayerAnimator;
    private GameObject ArmMover;
    private GameObject weppos;
    private GameObject PHand;
    private AimIK playerarm;
    public bool hit = false;
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
            //ArmMover.SetActive(true);
            ArmMover.transform.position = new Vector3(PHand.transform.position.x + rightJoystickInput.x, PHand.transform.position.y + rightJoystickInput.y);
            
            

            if (wepactive == false)
            {
                weapon.SetActive(true);
                wepactive = true;
            }
        }
        else
        {
            playerarm.enabled = false;
            ArmMover.transform.position = new Vector3(PHand.transform.position.x + rightJoystickInput.x, PHand.transform.position.y + rightJoystickInput.y);
            //ArmMover.SetActive(false);
            
            if (wepactive == true)
            {
                weapon.SetActive(false);
                wepactive = false;
            }
        }
            /*
            float animateweightstate = PlayerAnimator.GetLayerWeight(1);

            if(animateweightstate < 1.0f)
            {
                PlayerAnimator.SetLayerWeight(1, (animateweightstate + 0.1f));
            }
            
            GameObject weppos = GameObject.FindGameObjectWithTag("WepActiveLoc");
            Vector3 newpos = weppos.transform.position;
            Quaternion newrot = weppos.transform.rotation;

            //print(rightJoystickInput);
            
            if (wepactive == false)
            {
                weapon.SetActive(true);
                wepactive = true;
                if ((weapon.transform.position != newpos) || (weapon.transform.rotation != newrot))
                {
                    weapon.transform.SetPositionAndRotation(newpos, newrot);
                }
            }

            weapon.transform.SetPositionAndRotation(newpos, newrot);

            if (looking == 2)
            {
                PlayerAnimator.SetFloat("PX", rightJoystickInput.x);
                PlayerAnimator.SetFloat("PY", rightJoystickInput.y);
            }
            else
            {
                PlayerAnimator.SetFloat("PX", -rightJoystickInput.x);
                PlayerAnimator.SetFloat("PY", rightJoystickInput.y);
            }
            
        }
        else
        {
            float animateweightstate = PlayerAnimator.GetLayerWeight(1);

            if (animateweightstate > 0.0f)
            {
                PlayerAnimator.SetLayerWeight(1, (animateweightstate - 0.1f));
            }

            GameObject wepstorepos = GameObject.FindGameObjectWithTag("WepStoreLoc");
            Vector3 newpos = wepstorepos.transform.position;
            Quaternion newrot = wepstorepos.transform.rotation;

            
            if (wepactive == true)
            {
                weapon.SetActive(false);
                wepactive = false;
                if ((weapon.transform.position != newpos) || (weapon.transform.rotation != newrot))
                {
                    weapon.transform.SetPositionAndRotation(newpos, newrot);
                }
            }
            
            PlayerAnimator.SetFloat("PX", 0.0f);
            PlayerAnimator.SetFloat("PY", 0.0f);
        }
        */
    }

    private void FixedUpdate()
    {
        if (wepactive)
        {
            weapon.transform.position = weppos.transform.position;
            weapon.transform.LookAt(ArmMover.gameObject.transform.position);
        }
    }
}
