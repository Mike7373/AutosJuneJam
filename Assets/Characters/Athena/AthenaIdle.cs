using Characters;
using Input;
using UnityEngine;

public class AthenaIdle : MonoBehaviour
{

    ActionRunner actionRunner;
    GroundChecker groundChecker;

    CharacterInputAction jumpAction;
    CharacterInputAction punchAction;
    CharacterInputAction moveAction;

    void Awake()
    {
        actionRunner = GetComponent<ActionRunner>();
        groundChecker = GetComponent<GroundChecker>();

        var characterInput = GetComponent<CharacterInput>();
        jumpAction = characterInput.GetAction("Jump");
        punchAction = characterInput.GetAction("Punch");
        moveAction = characterInput.GetAction("Move");
        
        // Qui devo prima registrare gli handlers, perchè la coroutine mi fa un passaggio di stato 
        // nello stesso frame e non capisco perchè, la Stop non deregistra gli handlers
        // Forse con la versione a componenti con Start()  e Update() si risolve.
        jumpAction.performed += JumpActionOnperformed;
        punchAction.performed += PunchActionOnperformed;
    }

    void OnDestroy()
    {
        jumpAction.performed -= JumpActionOnperformed;
        punchAction.performed -= PunchActionOnperformed;
    }

    
    void PunchActionOnperformed(object f)
    {
        actionRunner.StartAction<AthenaPunch>();
    }
    
    
    public void JumpActionOnperformed(object f)
    {
        actionRunner.StartAction<AthenaJump>();
    }

    
    void FixedUpdate()
    {
        if (!groundChecker.IsGrounded())
        {
            actionRunner.StartAction<AthenaFalling>();
        }
        else
        {
            if (moveAction.IsInProgress())
            {
                actionRunner.StartAction<AthenaWalk>();
            }
        }
    }

}
