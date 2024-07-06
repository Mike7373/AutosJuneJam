using Characters.Zombie;
using UnityEngine;

public class ZombieIdle : MonoBehaviour
{
    ZombieBehaviour zombie;
    GroundChecker groundChecker;    
    
    void Awake()
    {
        groundChecker = GetComponent<GroundChecker>();
        zombie = GetComponent<ZombieBehaviour>();
        zombie.punchAction.performed += PunchActionOnperformed;
    }
    
    void OnDestroy()
    {
        zombie.punchAction.performed -= PunchActionOnperformed;
    }

    void PunchActionOnperformed(float obj)
    {
        zombie.StartAction<ZombiePunch>();
    }

    void FixedUpdate()
    {
        if (!groundChecker.IsGrounded())
        {
            zombie.StartAction<ZombieFalling>();
        }
        else if (zombie.moveAction.IsInProgress())
        {
            zombie.StartAction<ZombieWalk>();
        }
    }
}
