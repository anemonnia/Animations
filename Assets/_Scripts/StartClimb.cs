using UnityEngine;
using UnityEngine.InputSystem;

public class StartClimb : MonoBehaviour
{
    public PlayerMovement player;

    private bool inRange;

    public InputActionReference startClimbAction;

    private void OnEnable()
    {
        startClimbAction.action.Enable();
        startClimbAction.action.performed += StartAscension;
    }

    private void OnDisable()
    {
        startClimbAction.action.performed -= StartAscension;
        startClimbAction.action.Disable();
    }

    void StartAscension(InputAction.CallbackContext context)
    {
        if(inRange)
        {
            player.StartClimbing();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
