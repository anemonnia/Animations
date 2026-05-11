using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    //A refrence to the orientation which is used to tell the player what forward is
    public Transform orientation;
    //A refrence to the player
    public Transform player;
    //A refrence to the player mesh
    public Transform playerBody;

    //A refrence to the player's rigidbody
    public Rigidbody rb;

    //The value for the speed that the player rotates
    public float rotationSpeed;

    //Input values
    public InputActionReference moveAction;

    //The look at point of the aiming camera
    public Transform aimingLookAt;

    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerMovement>().isSpraying || player.GetComponent<PlayerMovement>().isClimbing)
        { 
            return;
        }

        moveInput = moveAction.action.ReadValue<Vector2>();

        //Calculates the camera's forward
        Vector3 lookDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        //Sets the orientation's forward to be the same as the camera's
        orientation.forward = lookDirection.normalized;

        //Finds the input direction
        Vector3 inputDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        //When the player is pressing an input key
        if (inputDirection != Vector3.zero)
        {
            if (moveInput.y >= 0)
            {
                //Smoothly rotates the player body in the direction of the input
                playerBody.forward = Vector3.Slerp(playerBody.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
            }
            else
            {
                //Calculates the camera's forward
                Vector3 aimingLookDirection = aimingLookAt.position - new Vector3(transform.position.x, aimingLookAt.position.y, transform.position.z);
                //Sets the orientation's forward to be the same as the camera's
                orientation.forward = aimingLookDirection.normalized;

                playerBody.forward = Vector3.Slerp(playerBody.forward, aimingLookDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
