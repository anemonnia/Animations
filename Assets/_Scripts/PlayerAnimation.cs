using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    public PlayerMovement player;

    public GameObject axe;

    public Transform axeIdlePos;
    public Transform axeAttackPos;

    public void SpawnAxe()
    {
        player.SpawnAxe();
    }

    public void SpawnHose()
    {
        player.SpawnHose();
    }

    public void MoveAxe()
    {
        axe.transform.position = axeAttackPos.position;
        axe.transform.rotation = axeAttackPos.rotation;
    }

    public void ResetAxe()
    {
        axe.transform.position = axeIdlePos.position;
        axe.transform.rotation = axeIdlePos.rotation;
    }

}
