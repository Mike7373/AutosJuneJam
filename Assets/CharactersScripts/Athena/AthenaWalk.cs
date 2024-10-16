using Characters;
using Input;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AthenaWalk : MonoBehaviour
{
    AthenaBehavior player;
    Rigidbody rigidBody;
    Animator animator;
    GroundChecker groundChecker;
    ActionRunner actionRunner;

    CharacterInputAction jumpAction;
    CharacterInputAction punchAction;
    CharacterInputAction runModifierAction;
    CharacterInputAction moveAction;
    EventInstance footsteps;
    
    void Awake()
    {
        actionRunner = GetComponent<ActionRunner>();
        player = GetComponent<AthenaBehavior>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        groundChecker = GetComponent<GroundChecker>();
        
        var characterInput = GetComponent<CharacterInput>();
        jumpAction = characterInput.GetAction("Jump");
        punchAction = characterInput.GetAction("Punch");
        moveAction = characterInput.GetAction("Move");
        runModifierAction = characterInput.GetAction("RunModifier");
        
        moveAction.canceled += MoveActionOncanceled;
        jumpAction.performed += JumpActionOnperformed;
        punchAction.performed += PunchActionOnperformed;
        
        animator.SetBool(AnimatorProperties.IsMoving, true);
        
        footsteps = RuntimeManager.CreateInstance(FMODEvents.instance.footsteps);
        footsteps.setVolume(0.2f);
    }

    void Start()
    {    
        footsteps.start();
        footsteps.release();
    }

    void OnDestroy()
    {
        rigidBody.velocity = Vector3.zero;
        moveAction.canceled -= MoveActionOncanceled;
        jumpAction.performed -= JumpActionOnperformed;
        punchAction.performed -= PunchActionOnperformed;
        animator.SetBool(AnimatorProperties.IsMoving, false);
        footsteps.stop(STOP_MODE.ALLOWFADEOUT);
    }
    
    void PunchActionOnperformed(object f)
    {
        actionRunner.StartAction<AthenaPunch>();
    }
    
    void JumpActionOnperformed(object f)
    {
        actionRunner.StartAction<AthenaJump>();
    }
    
    void MoveActionOncanceled()
    {
        actionRunner.StartAction<AthenaIdle>();
    }

    void FixedUpdate()
    {
        // TRANSAZIONE
        if (!groundChecker.IsGrounded())
        {
            actionRunner.StartAction<AthenaFalling>();
            return;
        }

        // Corsa
        bool speedModifier = runModifierAction.IsInProgress();
        float speed = speedModifier ? player.runSpeed : player.speed;
        Vector2 inputValue = moveAction.ReadValue<Vector2>();
        int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
        if (axisDirection != 0)
        {
            rigidBody.rotation = Quaternion.LookRotation(player.movementAxis * axisDirection, Vector3.up);
            rigidBody.velocity = speed * axisDirection * player.movementAxis;
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }
    }
}
