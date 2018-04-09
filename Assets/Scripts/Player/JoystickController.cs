using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using UnityEngine.AI;
using UnityEngine.UI;
using RWGameManager;

public class JoystickController : MonoBehaviour {
    public LeftJoystick leftJoystick;
    public RightJoystick rightJoystick;
    public bool hit = false;
    public bool SpeedGetRunning = false;
    public int looking = 2;
    public float swingspeed;
    public float time = 0;
    public float forcedZPos;
    public float jumpHeight;
    public float jumpDistance;
    public GameObject weapon;
    private bool wepactive = true;
    private Sword wep;
    private Vector3 playerRot;
    private Animator PlayerAnimator;
    private CharacterController charCont;
    private GameObject ArmMover;
    public GameObject weppos;
    private GameObject PHand;
    public Vector3 leftJoystickInput;
    public Vector3 rightJoystickInput;
    public int health = 10;
    public int maxHealth = 0;
    private AimIK playerarm;
    private bool moving = false;
    private bool falling = false;
    private bool movementAllowed = true;
    private float moveMagnitude;
    private NavMeshAgent playerAgent;
    public float movespeedMod = 6.0f;
    private Animation playerAnimsCont;
    private Animation[] playerAnims;
    private bool climbingStairs = false;
    private CapsuleCollider playerCol;
    public bool grounded = false;
    private bool reacting = false;
    private Vector3 veloc = Vector3.zero;
    public int currentWeaponIndex = 0;
    public GameObject[] weapons;
    public int maxWeapons = 5;
    private GameObject wepPanel;
    private GameManager gmanager;
    private Button[] wepbuts;
    public GameObject[] equipedWeps;
    public RectTransform healthBar;
    public int collectedGems = 0;
    public bool armored = false;
    public Material armoredmat;
    // Use this for initialization
    void Start () {
        healthBar = GameObject.Find("HealthBar").GetComponent<RectTransform>();
        gmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        maxHealth = health;
        playerAnimsCont = GetComponent<Animation>();
        playerCol = GetComponent<CapsuleCollider>();
        PlayerAnimator = GetComponent<Animator>();
        weppos = GameObject.FindGameObjectWithTag("WepActiveLoc");
        wepPanel = GameObject.FindGameObjectWithTag("WeaponsPanel");
        ArmMover = GameObject.FindGameObjectWithTag("ArmMover");
        PHand = ArmMover.GetComponent<ArmMover>().playerHand;
        playerarm = GetComponent<AimIK>();

        GameObject[] arr = GameObject.FindGameObjectsWithTag("PlayerWeapon");
        if(arr.Length < maxWeapons)
            weapons = new GameObject[arr.Length];
        else
            weapons = new GameObject[maxWeapons];

        wepbuts = new Button[wepPanel.GetComponentsInChildren<Button>(true).Length];
        wepbuts = wepPanel.GetComponentsInChildren<Button>(true);

        int count = 0;
        foreach (GameObject arrItem in arr)
        {
            if(arrItem.name == "SMG")
            {
                SMG script = arrItem.GetComponent<SMG>();
                weapons[script.weaponIndex] = arrItem;
                if(script.equiped)
                    wepbuts[script.weaponIndex].gameObject.SetActive(true);
            }

            if (arrItem.name == "Sword")
            {
                Sword script = arrItem.GetComponent<Sword>();
                weapons[script.weaponIndex] = arrItem;
                if (script.equiped)
                    wepbuts[script.weaponIndex].gameObject.SetActive(true);
            }

            if (arrItem.name == "Pistol")
            {
                Pistol script = arrItem.GetComponent<Pistol>();
                weapons[script.weaponIndex] = arrItem;
                if (script.equiped)
                    wepbuts[script.weaponIndex].gameObject.SetActive(true);
            }

            
            arrItem.SetActive(false);
            count++;
        }

        if (weapons[currentWeaponIndex] != null)
            weapon = weapons[currentWeaponIndex];
        else
            weapon = weapons[0];

    }
	
	// Update is called once per frame
	void Update () {
        rightJoystickInput = ArmMover.GetComponent<ArmMover>().maxDistFromX * rightJoystick.GetInputDirection();

        if (GetComponent<Rigidbody>().velocity.y < -4 || GetComponent<Rigidbody>().velocity.y > 4)
            grounded = false;

        if ((rightJoystickInput != Vector3.zero) || (hit))
        {
            playerarm.enabled = true;
            LeanTween.moveX(ArmMover, PHand.transform.position.x + (ArmMover.GetComponent<ArmMover>().maxDistFromX * rightJoystickInput.x),0.2f);
            LeanTween.moveY(ArmMover, PHand.transform.position.y + (ArmMover.GetComponent<ArmMover>().maxDistFromY * rightJoystickInput.y), 0.2f);
            //ArmMover.transform.position = new Vector3(PHand.transform.position.x + (ArmMover.GetComponent<ArmMover>().maxDistFromX * rightJoystickInput.x), PHand.transform.position.y + (ArmMover.GetComponent<ArmMover>().maxDistFromY * rightJoystickInput.y));

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

            if (wepactive == true)
            {
                weapon.SetActive(false);
                wepactive = false;
            }
        }

        weapon.transform.position = weppos.transform.position;
       // weapon.transform.LookAt(new Vector3(PHand.transform.position.x + (ArmMover.GetComponent<ArmMover>().maxDistFromX * rightJoystickInput.x), PHand.transform.position.y + (ArmMover.GetComponent<ArmMover>().maxDistFromY * rightJoystickInput.y), 0));
    }



    private void FixedUpdate()
    {
        healthBar.localScale = new Vector3(((float)health / (float)maxHealth),1,1);

        playerRot = new Vector3 (transform.rotation.x, transform.rotation.y, transform.rotation.z);
        PlayerAnimator.SetBool("Grounded", grounded);

        leftJoystickInput =  leftJoystick.GetInputDirection();
        rightJoystickInput = 2 * rightJoystick.GetInputDirection();

        if(health <= 0)
        {
            healthBar.localScale = new Vector3(0,1,1);
            GameObject.Find("CameraBox").GetComponent<CameraPusher>().enabled = false;
            StartCoroutine(Die());
        }
        
        if(Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        

        if(leftJoystickInput != Vector3.zero && movementAllowed)
        {
            moving = true;
            Vector3 joystickXOnly = new Vector3(leftJoystickInput.x, 0.0f, 0.0f);
            moveMagnitude = leftJoystickInput.x;

            if (moveMagnitude < 0)
                moveMagnitude = moveMagnitude * -1;

            if (leftJoystickInput.x > 0)
            {
                looking = 1;
            }
            else
            if(leftJoystickInput.x < 0)
            {
                looking = -1;
            }

            if(climbingStairs)
                PlayerAnimator.SetFloat("Forward", moveMagnitude * 0.8f, 0.2f,Time.deltaTime);
            else
                PlayerAnimator.SetFloat("Forward", moveMagnitude, 0.2f, Time.deltaTime);

            if (!grounded)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + (looking*0.05f), GetComponent<Rigidbody>().velocity.y, 0);
            }
            else
            {
                GetComponent<Rigidbody>().velocity = new Vector3(leftJoystickInput.x * movespeedMod, GetComponent<Rigidbody>().velocity.y, 0);
            }
        }
        else
        if (Input.GetAxis("Horizontal") != 0 && movementAllowed)
        {
            moving = true;
            Vector3 joystickXOnly = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
            moveMagnitude = Input.GetAxis("Horizontal");

            if (moveMagnitude < 0)
                moveMagnitude = moveMagnitude * -1;

            if (Input.GetAxis("Horizontal") > 0)
            {
                looking = 1;
            }
            else
            if (Input.GetAxis("Horizontal") < 0)
            {
                looking = -1;
            }

            if (climbingStairs)
                PlayerAnimator.SetFloat("Forward", moveMagnitude * 0.8f, 0.2f, Time.deltaTime);
            else
                PlayerAnimator.SetFloat("Forward", moveMagnitude, 0.2f, Time.deltaTime);

            if(!grounded)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x + (looking * 0.05f), GetComponent<Rigidbody>().velocity.y, 0);
            }
            else
            {
                GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Horizontal") * movespeedMod, GetComponent<Rigidbody>().velocity.y, 0);
            }
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
            moving = false;
            PlayerAnimator.SetFloat("Forward",0.0f, 0.2f, Time.deltaTime);
        }

        if(looking == -1 && transform.rotation.eulerAngles.y != 270.0f)
        {
            var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, 270.0f, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * 10f);
        }
        else
        if (looking == 1 && transform.rotation.eulerAngles.y != 90.0f)
        {
            var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, 90.0f, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * 10f);
        }


        if (transform.position.z != forcedZPos)
        {
            Vector3 NewPos = new Vector3(transform.position.x, transform.position.y, forcedZPos);
            transform.position = NewPos;
        }

        if(leftJoystickInput.y <= -0.8 && movementAllowed)
        {
            PlayerAnimator.SetBool("Crouched", true);
        }
        else
        {
            PlayerAnimator.SetBool("Crouched", false);
        }

        if (wepactive)
        {
            if (SpeedGetRunning == false)
                StartCoroutine(SpeedandDistance());

            //weapon.transform.position = weppos.transform.position;

            
        }

        if (!grounded && !falling)
            StartCoroutine(Falling());

        if (grounded)
            StopCoroutine(Falling());
        
        
    }

    public void WepChange(int wepIndex)
    {
        foreach(GameObject wep in weapons)
        {
            wep.SetActive(false);
        }
        
        weapon = weapons[wepIndex];
    }

    public void ReturnWepSpeed()
    {
        StartCoroutine(SpeedandDistance());
    }

    public void Jump()
    {
        if (grounded)
        {
            StartCoroutine(Jumper());
        }
        
    }

    IEnumerator Falling()
    {
        falling = true;
        int counter = 0;
        int numNotG = 0;
        while(counter < 16)
        {
            counter++;
            yield return new WaitForSeconds(0.1f);

            if (!grounded)
                numNotG++;
        }

        if (numNotG >= 15)
            falling = true;
        else
            falling = false;

        if (falling)
        {
            movementAllowed = false;
            PlayerAnimator.enabled = false;
            yield return new WaitUntil(() => grounded == true);
            PlayerAnimator.enabled = true;
            PlayerAnimator.Play(Animator.StringToHash("standing_up_from_belly"));
            yield return new WaitForSecondsRealtime(1.5f);
            movementAllowed = true;
        }
        falling = false;
    }

    IEnumerator Jumper()
    {
        
        PlayerAnimator.Play(Animator.StringToHash("Jump"));
        if(moving)
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpHeight, 0.0f);
        else
            GetComponent<Rigidbody>().velocity = new Vector3(jumpDistance*looking, jumpHeight, 0.0f);

        PlayerAnimator.SetBool("Grounded", false);
        yield return new WaitForSecondsRealtime(1.0f);
        yield return new WaitUntil(() => grounded == true);
    }

    IEnumerator HitReact()
    {
        if(!armored)
        {
            reacting = true;
            PlayerAnimator.Play("Impact");
            health--;
            Handheld.Vibrate();
            float wait = PlayerAnimator.GetCurrentAnimatorClipInfo(0).Length;
            yield return new WaitForSecondsRealtime(wait);
            reacting = false;
        }
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

    IEnumerator Die()
    {
        PlayerAnimator.enabled = false;
        movementAllowed = false;
        yield return new WaitForSecondsRealtime(4.0f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().reload_scene();
    }

    void Equip(int type,string obj)
    {
        if(type == 1)
        {
            int c = 0;
            foreach (GameObject wep in weapons)
            {
                if (wep.name == obj)
                {
                    wepbuts[c].gameObject.SetActive(true);
                    WepChange(c);
                }

                c++;
                
            }
        }
        else
        if(type == 2)
        {
            collectedGems++;
            gmanager.GemCollect();
        }
        else
        if (type == 3)
        {
            StartCoroutine(ArmorUp());
        }
        else
        if(type == 4)
        {
            if(health < maxHealth)
                health++;
        }

    }
    
    IEnumerator ArmorUp()
    {
        armored = true;
        Material orig = GetComponentInChildren<SkinnedMeshRenderer>().material;
        GetComponentInChildren<SkinnedMeshRenderer>().material = armoredmat;
        yield return new WaitForSecondsRealtime(15.0f);
        GetComponentInChildren<SkinnedMeshRenderer>().material = orig;
        armored = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Stairs")
        {
            GetComponent<Rigidbody>().drag = 4.0f;
            GetComponent<Rigidbody>().AddForce(Vector3.down * 10);
            StopCoroutine(Falling());
            climbingStairs = true;
            grounded = true;
        }
        
        if ((collision.gameObject.layer == 9)||(collision.gameObject.tag == "Ground"))
        {
            StopCoroutine(Falling());
            grounded = true;
        }
            

        if(collision.gameObject.tag == "EnemyWeapon" && !reacting)
        {
            StartCoroutine(HitReact());
        }

        if (collision.gameObject.tag == "Killer")
        {
            StartCoroutine(Die());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            Pickup item = other.gameObject.GetComponent<Pickup>();
            Equip(item.itemType, item.itemName);
            other.gameObject.GetComponent<Pickup>().itemCollected();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Stairs")
        {
            GetComponent<Rigidbody>().drag = 4.0f;
            GetComponent<Rigidbody>().AddForce(Vector3.down * 10);
            StopCoroutine(Falling());
            climbingStairs = true;
            grounded = true;
        }

        if ((collision.gameObject.layer == 9) || (collision.gameObject.tag == "Ground"))
        {
            StopCoroutine(Falling());
            grounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Stairs")
        {
            GetComponent<Rigidbody>().drag = 0.0f;
            climbingStairs = false;
        }

        if (collision.gameObject.layer == 9 && !climbingStairs && GetComponent<Rigidbody>().velocity.y < -3)
        {
            grounded = false;
        }
            
    }

}
