using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	[RequireComponent(typeof(IBzThirdPerson))]
	public sealed class BzThirdPersonControl : MonoBehaviour
	{
		private IBzThirdPerson _character;
		private IBzRagdoll _ragdoll;
		private IBzDamageable _health;
		private Transform _camTransform;
		private bool _jumpPressed;
		private bool _fire;
		private bool _crouch;
        public Transform leftlook;
        public Transform rightlook;
        public Transform curlook;
        private JoystickController joyCon;
        private float walkline;
        private float farline;
        private float h = 0.0f;
        private void Start()
		{
            joyCon = GetComponent<JoystickController>();
            walkline = 0;
            farline = 0;
            _camTransform = curlook.transform;

			_character = GetComponent<IBzThirdPerson>();
			_health = GetComponent<IBzDamageable>();
			_ragdoll = GetComponent<IBzRagdoll>();
		}

		void Update()
		{
			// read user input: jump, fire and crouch

			if (!_jumpPressed)
				_jumpPressed = Input.GetButtonDown("Jump");
			if (!_fire)
				_fire = Input.GetMouseButtonDown(0);
            
			_crouch = Input.GetKey(KeyCode.C);

            if ((transform.position.z != walkline) && (transform.position.z != farline) && (h != 0))
            {
                if (h == 0.0f)
                {
                    if (joyCon.looking == 1)
                        transform.position = new Vector3(transform.position.x, transform.position.y, farline);
                    else
                        transform.position = new Vector3(transform.position.x, transform.position.y, walkline);
                }
                else
                {
                    if ((joyCon.looking == 2) && (transform.position.z != walkline))
                    {
                        Vector3 NewPos = new Vector3(transform.position.x, transform.position.y, walkline);
                        Vector3 velocity = default(Vector3);
                        transform.position = Vector3.SmoothDamp(transform.position, NewPos, ref velocity, Time.deltaTime * 10);
                    }

                    if ((joyCon.looking == 1) && (transform.position.z != farline))
                    {
                        Vector3 NewPos = new Vector3(transform.position.x, transform.position.y, farline);
                        Vector3 velocity = default(Vector3);
                        transform.position = Vector3.SmoothDamp(transform.position, NewPos, ref velocity, Time.deltaTime * 10);
                    }
                }
            }
        }

        public void Jump()
        {
            _jumpPressed = true;
        }

		private void FixedUpdate()
		{
			// read user input: movement
			h = Input.GetAxis("Horizontal");

            if(h == 0)
            {
                h = joyCon.leftJoystickInput.x;
            }

            if(joyCon.leftJoystickInput.y >= 0.8)
            {
                _jumpPressed = true;
            }

            if (joyCon.leftJoystickInput.y <= -0.8)
            {
                _crouch = true;
            }

            float v = 0;

            Vector3 camForward = new Vector3(0,0,0);
            _camTransform = curlook.transform;

            // calculate move direction and magnitude to pass to character
            if (h < 0)
            {
                camForward = new Vector3(-100, 0, 0).normalized;
                _camTransform = leftlook.transform;
                joyCon.looking = 1;
            }
                
            if (h > 0)
            {
                camForward = new Vector3(100, 0, 0).normalized;
                _camTransform = rightlook.transform;
                joyCon.looking = 2;
            }            

            
                
            

            Vector3 move = v * camForward + h * _camTransform.right;
			if (move.magnitude > 1)
				move.Normalize();

			ProcessDamage();

			// pass all parameters to the character control script
			_character.Move(move, _crouch, _jumpPressed);
			_jumpPressed = false;

			// if ragdolled, add a little move
			if (_ragdoll != null && _ragdoll.IsRagdolled)
				_ragdoll.AddExtraMove(move * 100 * Time.deltaTime);
            
		}

        /// <summary>
        /// if health script attached, shot the character
        /// </summary>
        private void ProcessDamage()
		{
			if (_health == null)
				return;
            
			if (_fire)
            {
               // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //_health.Shot(ray, 0.40f, 10000f);
				_fire = false;
			}

			if (_jumpPressed && _health.IsDead())
			{
				_health.Health = 1f;
				_jumpPressed = false;
			}
		}
	}
}