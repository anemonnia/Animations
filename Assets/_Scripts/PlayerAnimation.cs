using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    public Animator animator;

    [ContextMenu("Trigger Axe Swing")]
    public void TriggerAxeSwing()
    {
        animator.SetTrigger("AxeSwing");
    }

    public void TriggerLift()
    { 
        animator.SetTrigger("")
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            TriggerAxeSwing();
        }
    }

}
