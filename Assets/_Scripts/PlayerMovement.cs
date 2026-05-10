using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //The value for the player's walk speed
    public float walkSpeed;
    //The value for the player's run speed
    public float runSpeed;
    //A value for the drag applied to the player while moving
    public float drag;
    //Value for the player's total movement speed
    private float movementSpeed;

    //Refrence to the orientation
    public Transform orientation;
    //The direction the player moves in
    private Vector3 movementDirection;
    //A refrence to the rigidbody
    private Rigidbody rb;

    //The player's height, used for ground checks
    public float playerHeight;
    //A bool that tells you if you are on the ground
    private bool onGround;
    //A refrence to a layermask used for ground checks
    public LayerMask whatIsGround;

    //The max slope angle that the player can move up
    public float maxSlopeAngle;
    //A raycast for checking for slopes
    private RaycastHit hitSlope;
    //A bool to make sure you can jump normally while in a slope
    private bool exitingSlope;

    //The input values
    public InputActionReference moveAction;
    public InputActionReference runAction;

    private Vector2 moveInput;

    public MovementState state;

    public Animator animator;
    public enum MovementState
    {
        walking,
        running,
        inAir
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        runAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        runAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if the player is on the ground
        onGround = Physics.Raycast(transform.position, Vector3.down, (playerHeight * 0.5f) + 0.2f, whatIsGround);

        //Calling the input function
        MyInput();
        //Calling the speed control
        ControlSpeed();
        //Calling the state handler
        StateHandler();

        //If the player is touching the ground
        if (onGround)
        {
            //Applies drag to the player
            rb.linearDamping = drag;
        }
        //If the player is not touching the ground
        else
        {
            //Has no drag on the player
            rb.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        //Calling the movement function
        MovePlayer();
    }

    //Method that gets the inputs
    private void MyInput()
    {
        //Gets the inputs
        moveInput = moveAction.action.ReadValue<Vector2>();
    }

    private void StateHandler()
    {
        bool isRunning = runAction.action.IsPressed();

        //If the player is running
        if (onGround && isRunning)
        {
            state = MovementState.running;
            movementSpeed = runSpeed;
        }
        //If the player is walking
        else if (onGround)
        {
            state = MovementState.walking;

            movementSpeed = walkSpeed;
        }
        //If the player is in the air
        else
        {
            state = MovementState.inAir;
        }
    }

    //Method that moves the player
    private void MovePlayer()
    {
        //Calcualtes the movement direction based on the orientation and input
        movementDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        //If the player is on a slope
        if (OnSlope() && !exitingSlope)
        {
            //Adds force in the slope direction
            rb.AddForce(GetSlopeMoveDirection() * movementSpeed * 20f, ForceMode.Force);

            //Adds force down to make sure player doesn't bump up and down while moving on slope
            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //Adds a force in the direction the input is
        rb.AddForce(movementDirection.normalized * movementSpeed * 10f, ForceMode.Force);

        //Turn off the gravity while the player is on a slope, to prevent him from sliding down
        rb.useGravity = !OnSlope();
    }

    //A method that makes sure the player doesn't go too fast
    private void ControlSpeed()
    {
        //For limiting speed on slopes
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > movementSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * movementSpeed;
            }
        }
        //For limiting the speed everywhere else
        else
        {
            //Gets the player's flat velocity
            Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            //Limit the player's velocity when it is needed
            if (flatVelocity.magnitude > movementSpeed)
            {
                //Creates a new vector for limiting player velocity
                Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;

                //Applies the limited velocity to the rigidbody
                rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
            }
        }
    }

    //For checking if the player is on a slope
    private bool OnSlope()
    {
        //Sends a raycast down to see if the player is on a slope
        if (Physics.Raycast(transform.position, Vector3.down, out hitSlope, (playerHeight * 0.5f) + 0.3f))
        {
            //Checks the angle of the slope
            float angle = Vector3.Angle(Vector3.up, hitSlope.normal);
            //Returns true if the angle is less than the max angle but also not 0
            return angle < maxSlopeAngle && angle != 0;
        }
        //If it doesn't hit a slope, it returns false
        return false;
    }

    //Projecting the movement speed to match with the slope
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(movementDirection, hitSlope.normal).normalized;
    }
}
//controller = GetComponent<CharacterController>();
//    [Header("Animation")]
//    public Animator animator;
