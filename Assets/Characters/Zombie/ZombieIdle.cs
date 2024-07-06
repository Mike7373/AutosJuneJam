using Characters;
using Characters.Zombie;
using Input;
using UnityEngine;

public class ZombieIdle : MonoBehaviour
{
    ActionRunner actionRunner;
    GroundChecker groundChecker;
    
    CharacterInputAction<float> punchAction;
    CharacterInputAction<Vector2> moveAction;
    
    void Awake()
    {
        groundChecker = GetComponent<GroundChecker>();
        actionRunner = GetComponent<ActionRunner>();
        
        var characterInput = GetComponent<CharacterInput>();
        punchAction = characterInput.GetAction<float>("Punch");
        moveAction = characterInput.GetAction<Vector2>("Move");
        
        punchAction.performed += PunchActionOnperformed;
    }
    
    void OnDestroy()
    {
        punchAction.performed -= PunchActionOnperformed;
    }

    void PunchActionOnperformed(float obj)
    {
        actionRunner.StartAction<ZombiePunch>();
    }

    void FixedUpdate()
    {
        if (!groundChecker.IsGrounded())
        {
            actionRunner.StartAction<ZombieFalling>();
        }
        else if (moveAction.IsInProgress())
        {
            actionRunner.StartAction<ZombieWalk>();
        }
    }
}
