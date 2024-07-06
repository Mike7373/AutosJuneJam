using Characters;
using Characters.Zombie;
using UnityEngine;

public class ZombieWalk : MonoBehaviour
{
    Vector3 movementDirection;
    ZombieBehaviour zombie;
    GroundChecker groundChecker;    

    void Awake()
    {
        zombie = GetComponent<ZombieBehaviour>();
        groundChecker = GetComponent<GroundChecker>();
        zombie.moveAction.canceled += MoveActionOncanceled;
        zombie.punchAction.performed += PunchActionOnperformed;
        zombie.animator.SetBool(AnimatorProperties.IsMoving, true);
    }

    void PunchActionOnperformed(float obj)
    {
        zombie.StartAction<ZombiePunch>();
    }

    void OnDisable()
    {
        zombie.moveAction.canceled -= MoveActionOncanceled;
        zombie.punchAction.performed -= PunchActionOnperformed;
        zombie.rigidBody.velocity = Vector3.zero;
        zombie.animator.SetBool(AnimatorProperties.IsMoving, false);
    }
    
    void FixedUpdate()
    {
        // TRANSAZIONE
        if (!groundChecker.IsGrounded())
        {
            zombie.StartAction<ZombieFalling>();
            return;
        }
        
        // LAVORO
        bool speedModifier = zombie.runModifierAction.IsInProgress();
        float speed = speedModifier ? zombie.runSpeed : zombie.speed;
        Vector2 inputValue = zombie.moveAction.ReadValue();
        int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
        if (axisDirection != 0)
        {
            zombie.rigidBody.rotation = Quaternion.LookRotation(zombie.movementAxis * axisDirection, Vector3.up);
            zombie.rigidBody.velocity = speed * axisDirection * zombie.movementAxis;
        }
        else
        {
            zombie.rigidBody.velocity = Vector3.zero;
        }
    }
    
    void MoveActionOncanceled()
    {
        zombie.StartAction<ZombieIdle>();
    }
}
