using UnityEngine;

public class StopClimb : MonoBehaviour
{
    public PlayerMovement player;
    public Transform platform;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if(player.isClimbing)
            {
                player.transform.position = platform.position;
                player.StopClimbing();
            }
        }

    }


}
