using Characters;
using Characters.Zombie;
using Input;
using UnityEngine;

public class ZombieIdle : MonoBehaviour
{
    ActionRunner actionRunner;
    CharacterController movementController;
    
    CharacterInputAction punchAction;
    CharacterInputAction moveAction;
    
    void Awake()
    {
        movementController = GetComponent<CharacterController>();
        actionRunner = GetComponent<ActionRunner>();
        var characterInput = GetComponent<CharacterInput>();
        punchAction = characterInput.GetAction("Punch");
        moveAction = characterInput.GetAction("Move");
        punchAction.performed += PunchActionOnperformed;
    }
    
    void OnDestroy()
    {
        punchAction.performed -= PunchActionOnperformed;
    }

    void PunchActionOnperformed(object _)
    {
        actionRunner.StartAction<ZombiePunch>();
    }

    void Update()
    {
        if (!movementController.isGrounded)
        {
            actionRunner.StartAction<ZombieFalling>();
        }
        else if (moveAction.IsInProgress())
        {
            actionRunner.StartAction<ZombieWalk>();
        }
    }
}
