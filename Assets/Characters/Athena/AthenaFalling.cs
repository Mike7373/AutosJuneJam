using Characters;
using Input;
using UnityEngine;

public class AthenaFalling : MonoBehaviour
{
    AthenaBehavior player;
    Rigidbody rigidBody;
    GroundChecker groundChecker;
    ActionRunner actionRunner;
    
    CharacterInputAction<float> runModifierAction;
    CharacterInputAction<Vector2> moveAction;
    
    void Awake()
    {
        player = GetComponent<AthenaBehavior>();
        actionRunner = GetComponent<ActionRunner>();
        rigidBody = GetComponent<Rigidbody>();
        groundChecker = GetComponent<GroundChecker>();
        
        var characterInput = GetComponent<CharacterInput>();
        runModifierAction = characterInput.GetAction<float>("RunModifier");
        moveAction = characterInput.GetAction<Vector2>("Move");
    }
    
    void OnDestroy()
    {
        rigidBody.useGravity = false;
        rigidBody.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        rigidBody.useGravity = true;
        if (groundChecker.IsGrounded())
        {
            actionRunner.StartAction<AthenaIdle>();
        }
        else
        {
            // In volo mi muovo
            Vector2 inputValue = moveAction.ReadValue();
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            bool speedModifier = runModifierAction.IsInProgress();
            float speed = speedModifier ? player.runSpeed : player.speed;
            if (axisDirection != 0)
            {
                Vector3 velocity = speed * axisDirection * player.movementAxis;
                rigidBody.rotation = Quaternion.LookRotation(player.movementAxis * axisDirection, Vector3.up);
                rigidBody.velocity = new Vector3(velocity.x, rigidBody.velocity.y, velocity.z);
            }
            else
            {
                rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
            }
        }
    }
}
