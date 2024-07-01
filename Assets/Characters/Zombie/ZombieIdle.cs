using UnityEngine;

public class ZombieIdle : MonoBehaviour
{
    ZombieBehaviour zombie;
    
    void Start()
    {
        zombie = GetComponent<ZombieBehaviour>();
    }

    void FixedUpdate()
    {
        if (!zombie.IsGrounded())
        {
            zombie.StartAction<ZombieFalling>();
        }
        else if (zombie.moveAction.IsInProgress())
        {
            zombie.StartAction<ZombieWalk>();
        }
    }
}
