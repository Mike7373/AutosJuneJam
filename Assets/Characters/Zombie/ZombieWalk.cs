using Characters;
using Characters.Zombie;
using Input;
using UnityEngine;

public class ZombieWalk : MonoBehaviour
{
    Vector3 movementDirection;
    ZombieBehaviour zombie;
    GroundChecker groundChecker;
    Animator animator;
    ActionRunner actionRunner;
    Rigidbody rigidBody;
    
    CharacterInputAction<float> punchAction;
    CharacterInputAction<float> runModifierAction;
    CharacterInputAction<Vector2> moveAction;

    void Awake()
    {
        zombie = GetComponent<ZombieBehaviour>();
        groundChecker = GetComponent<GroundChecker>();
        animator = GetComponent<Animator>();
        actionRunner = GetComponent<ActionRunner>();
        rigidBody = GetComponent<Rigidbody>();
        
        var characterInput = GetComponent<CharacterInput>();
        punchAction = characterInput.GetAction<float>("Punch");
        moveAction = characterInput.GetAction<Vector2>("Move");
        runModifierAction = characterInput.GetAction<float>("RunModifier");
        
        moveAction.canceled += MoveActionOncanceled;
        punchAction.performed += PunchActionOnperformed;
        animator.SetBool(AnimatorProperties.IsMoving, true);
    }

    void PunchActionOnperformed(float obj)
    {
        actionRunner.StartAction<ZombiePunch>();
    }

    void OnDisable()
    {
        moveAction.canceled -= MoveActionOncanceled;
        punchAction.performed -= PunchActionOnperformed;
        rigidBody.velocity = Vector3.zero;
        animator.SetBool(AnimatorProperties.IsMoving, false);
    }
    
    void FixedUpdate()
    {
        // TRANSAZIONE
        if (!groundChecker.IsGrounded())
        {
            actionRunner.StartAction<ZombieFalling>();
            return;
        }
        
        // LAVORO
        bool speedModifier = runModifierAction.IsInProgress();
        float speed = speedModifier ? zombie.runSpeed : zombie.speed;
        Vector2 inputValue = moveAction.ReadValue();
        int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
        if (axisDirection != 0)
        {
            rigidBody.rotation = Quaternion.LookRotation(zombie.movementAxis * axisDirection, Vector3.up);
            rigidBody.velocity = speed * axisDirection * zombie.movementAxis;
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }
    }
    
    void MoveActionOncanceled()
    {
        actionRunner.StartAction<ZombieIdle>();
    }
}
