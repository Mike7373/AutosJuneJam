using Characters;
using Input;
using UnityEngine;

public class ZombieFalling : MonoBehaviour
{
    ZombieBehaviour zombie;
    Rigidbody rigidBody;
    GroundChecker groundChecker;
    ActionRunner actionRunner;
    
    CharacterInputAction runModifierAction;
    CharacterInputAction moveAction;

    void Awake()
    {
        zombie = GetComponent<ZombieBehaviour>();
        groundChecker = GetComponent<GroundChecker>();
        actionRunner = GetComponent<ActionRunner>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        
        var characterInput = GetComponent<CharacterInput>();
        moveAction = characterInput.GetAction("Move");
        runModifierAction = characterInput.GetAction("RunModifier");
    }

    void OnDisable()
    {
        rigidBody.useGravity = false;
        rigidBody.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (groundChecker.IsGrounded())
        {
            actionRunner.StartAction<ZombieIdle>();
        }
        else
        {
            // In volo mi muovo
            Vector2 inputValue = moveAction.ReadValue<Vector2>();
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            bool speedModifier = runModifierAction.IsInProgress();
            float speed = speedModifier ? zombie.runSpeed : zombie.speed;
            if (axisDirection != 0)
            {
                Vector3 velocity = speed * axisDirection * zombie.movementAxis;
                rigidBody.rotation = Quaternion.LookRotation(zombie.movementAxis * axisDirection, Vector3.up);
                rigidBody.velocity = new Vector3(velocity.x, rigidBody.velocity.y, velocity.z);
            }
            else
            {
                rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
            }
        }
    }
}
