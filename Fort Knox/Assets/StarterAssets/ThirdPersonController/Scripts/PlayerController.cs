using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    private int _speed;

    [SerializeField]
    private int _walkSpeed = 5;

    [SerializeField]
    int runSpeed;

    [SerializeField]
    private float rotationSpeed = 2f;

    //private Transform cameraMain;
    [SerializeField] Transform cameraMain;
    public GameObject playerModel;
    private Animator _animator;

    private bool _runAnim = false;
    private bool _canMove = true;

    private LayerMask environment;

    private Collider[] raycastHitCache = new Collider[4];

    private bool grounded;

    private float groundCheckTimer = 0f;

    public GameObject hitBox_0;
    public GameObject hitBox_1;

    float tAngle;

    //private ObjectPickUp _objectPickup;

    private bool _canThrow = false;
    private bool thrown = true;

    private Quaternion _cameraDirection;

    private void Awake()
    {
        environment = 1 << LayerMask.NameToLayer("Environment");
    }

    void Start()
    {
        _speed = _walkSpeed;
        _characterController = gameObject.GetComponent<CharacterController>();
        //cameraMain = Camera.main.transform;
        //_objectPickup = gameObject.GetComponent<ObjectPickUp>();
        //_animator = playerModel.GetComponent<Animator>();

        //hitBox_0.gameObject.SetActive(false);
        //hitBox_1.gameObject.SetActive(false);
    }

    void Update()
    {
        groundCheckTimer += Time.deltaTime;
        if (groundCheckTimer > 0.25f)
        {
            grounded = IsGrounded();
            groundCheckTimer = 0;
        }

        //RUN-------------------------------------------
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _runAnim = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _runAnim = false;
        }
        //----------------------------------------------

        PlayerMovement();

        /*
        //INPUT-----------------------------------------
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_canMove)
            {
                _animator.SetTrigger("Punch_0");
                hitBox_0.gameObject.SetActive(true);
            } _canMove = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (_canMove)
            {
                _animator.SetTrigger("Kick_0");
                hitBox_1.gameObject.SetActive(true);
            } _canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_canMove)
            {
                _animator.SetTrigger("Punch_1");
                hitBox_0.gameObject.SetActive(true);
            } _canMove = false;
        }
        //----------------------------------------------
        

        //PICKUP----------------------------------------
        if (Input.GetKeyDown(KeyCode.F) && thrown)
        {
            //_objectPickup.GetNearObjects();
            thrown = false;
            Invoke("PickupSwitch", 0.2f);
        }

        if (Input.GetKeyUp(KeyCode.F) && _canThrow)
        {
            //_objectPickup.Throw();
            _canThrow = false;
            thrown = true;
        }

        if (Input.GetKeyDown(KeyCode.G) && _canThrow)
        {
            //_objectPickup.Shield();
        }
        //----------------------------------------------

        */
    }

    /*
    private void PickupSwitch()
    {
        _canThrow = true;
    }
    */


    private void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, Vertical);

        if (_canMove)
        {
            direction = cameraMain.forward * direction.z + cameraMain.right * direction.x;
        }
        else
        {
            direction = new Vector3(0, 0, 0);
        }

        if (direction.magnitude >= 0.1f)
        {
            direction.y -= 5f;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            _characterController.Move(direction * _speed * Time.deltaTime);

            tAngle = targetAngle;
            _cameraDirection = rotation;
        }
        //MoveAnimation(direction);
    }

    public Quaternion CameraDirection()
    {
        return _cameraDirection;
    }

    private bool IsGrounded()
    {
        int contacts = Physics.OverlapSphereNonAlloc(transform.position, 0.3f, raycastHitCache, environment);
        return contacts != 0;
    }

    /*
    private void MoveAnimation(Vector3 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            if (_runAnim)
            {
            _speed = runSpeed;
            AnimationStuff(false, "Walk");
            AnimationStuff(true, "Run");
            }
            if (!_runAnim)
            {
            _speed = _walkSpeed;
            AnimationStuff(false, "Run");
            AnimationStuff(true, "Walk");
            }
        }

        if (direction.magnitude < 0.1f)
        {
            AnimationStuff(false, "Walk");
            AnimationStuff(false, "Run");
        }
    }
    */
        public void AttackDone()
    {
        _canMove = true;
        hitBox_0.gameObject.SetActive(false);
        hitBox_1.gameObject.SetActive(false);
    }
    /*
    private void AnimationStuff(bool condition, string type)
    {
        if (condition == true && type == "Walk")
        {
            _animator.SetBool("RunBool", false);
            _animator.SetBool("WalkBool", true);
        }
        if (condition == false && type == "Walk")
        {
            _animator.SetBool("WalkBool", false);
        }
        if (condition == true && type == "Run")
        {
            _animator.SetBool("WalkBool", false);
            _animator.SetBool("RunBool", true);
        }
        if (condition == false && type == "Run")
        {
            _animator.SetBool("RunBool", false);
        }
    }
    */
    public float GiveSpeed()
    {return _speed;}
}
