using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickUp : MonoBehaviour
{
    public bool isAxe;
    public bool isHose;

    public PlayerMovement player;

    public bool inRange;

    public InputActionReference pickupAction;

    public MeshRenderer axeObject;

    public MeshRenderer hoseObject;

    public MeshRenderer nozzleObject;

    public bool isPicked;

    private void OnEnable()
    {
        pickupAction.action.Enable();
        pickupAction.action.performed += Pickup;
    }

    private void OnDisable()
    {
        pickupAction.action.performed -= Pickup;
        pickupAction.action.Disable();
    }

    private void Pickup(InputAction.CallbackContext context)
    {
        if(!this.inRange || this.isPicked || player.isPicking)
        {
            Debug.Log("Not working");
            return;
        }

        if (isAxe == true)
        {
            player.PickUpAxe();
            hoseObject.enabled = true;
            nozzleObject.enabled = true;
            hoseObject.GetComponentInParent<ItemPickUp>().isPicked = false;
            isPicked = true;
            inRange = false;
            player.isPicking = true;
            axeObject.enabled = false;
        }
        else if (isHose == true)
        {
            player.PickUpHose();
            axeObject.enabled = true;
            axeObject.GetComponentInParent<ItemPickUp>().isPicked = false;
            isPicked = true;
            inRange = false;
            player.isPicking = true;
            hoseObject.enabled = false;
            nozzleObject.enabled = false;
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
