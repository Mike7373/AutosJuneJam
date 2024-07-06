using System.Collections;
using Characters;
using Input;
using UnityEngine;

public class AthenaJump : MonoBehaviour
{
    AthenaBehavior player;
    ActionRunner actionRunner;
    Animator animator;
    Rigidbody rigidBody;
    
    CharacterAction<float> jumpAction;
    CharacterAction<Vector2> moveAction;
    CharacterAction<float> runModifierAction;

    
    void Awake()
    {
        player = GetComponent<AthenaBehavior>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        actionRunner = GetComponent<ActionRunner>();
        
        var characterInput = GetComponent<CharacterInput>();
        runModifierAction = characterInput.GetAction<float>("RunModifier");
        moveAction = characterInput.GetAction<Vector2>("Move");
        jumpAction = characterInput.GetAction<float>("Jump");
        
        jumpAction.canceled += EndJumpInput;
        animator.SetTrigger(AnimatorProperties.JumpTrigger);
        StartCoroutine(JumpCoroutine());
    }
    
    void OnDestroy()
    {
        jumpAction.canceled -= EndJumpInput;
        rigidBody.velocity = Vector3.zero;
    }

    void EndJumpInput()
    {
        actionRunner.StartAction<AthenaFalling>();
    }

    
    /**
     * Il salto imposta la velocità direttamente, finchè si tiene premuto il tasto di salto questa velocità viene
     * mantenuta. Se viene rilasciato prima, si cade prima. Serve a fare anche i saltini.
     * TODO: Forse meglio sostituire jumpDistance con jumpTime.
     */
    IEnumerator JumpCoroutine()
    {
        float jumpDistance = 0;
        float lastPos = rigidBody.position.y;
        while (jumpDistance < player.jumpRange)
        {
            // In volo mi muovo
            Vector3 velocity = Vector3.zero;
            Vector2 inputValue = moveAction.ReadValue();
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            bool speedModifier = runModifierAction.IsInProgress();
            float speed = speedModifier ? player.runSpeed : player.speed;
            if (axisDirection != 0)
            {
                rigidBody.rotation = Quaternion.LookRotation(player.movementAxis * axisDirection, Vector3.up);
                velocity = speed * axisDirection * player.movementAxis;
            }

            // Finchè sto saltando alzo la mia posizione
            velocity += new Vector3(0, player.jumpSpeed, 0);
            rigidBody.velocity = velocity;
            
            yield return new WaitForFixedUpdate();
            jumpDistance += rigidBody.position.y - lastPos;
            lastPos = rigidBody.position.y;
        }
        actionRunner.StartAction<AthenaFalling>();
    }
    
}
