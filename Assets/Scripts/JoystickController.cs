using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool hit = false;
    // Use this for initialization
    void Start () {
        PlayerAnimator = GetComponent<Animator>();
        GameObject wepGO = GameObject.FindGameObjectWithTag("PlayerWeapon");
        wep = wepGO.GetComponent<Sword>();
        weapon = GameObject.Find("Sword");
    }
	
	// Update is called once per frame
	void Update () {
        playerRot = new Vector3 (transform.rotation.x, transform.rotation.y, transform.rotation.z);

        leftJoystickInput = leftJoystick.GetInputDirection();
        rightJoystickInput = rightJoystick.GetInputDirection();
        
        if (rightJoystickInput != Vector3.zero)
        {
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

    }
}
