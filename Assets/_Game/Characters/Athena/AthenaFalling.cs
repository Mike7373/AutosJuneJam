using Characters;
using Input;
using UnityEngine;

public class AthenaFalling : MonoBehaviour
{
    Walker walker;
    CharacterController movementController;
    ActionRunner actionRunner;
    
    CharacterInputAction runModifierAction;
    CharacterInputAction moveAction;
    Animator animator;
    
    void Awake()
    {
        walker       = GetComponent<Walker>();
        actionRunner = GetComponent<ActionRunner>();
        animator     = GetComponent<Animator>();
        movementController = GetComponent<CharacterController>();
        var characterInput = GetComponent<CharacterInput>();
        runModifierAction = characterInput.GetAction("RunModifier");
        moveAction        = characterInput.GetAction("Move");
    }
    
    void Update()
    {
        if (movementController.isGrounded)
        {
            actionRunner.StartAction<AthenaIdle>();
            animator.SetBool(AnimatorProperties.IsGrounded, true);
        }
        else
        {
            // In volo mi muovo
            Vector2 inputValue = moveAction.ReadValue<Vector2>();
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            bool speedModifier = runModifierAction.IsInProgress();
            float speed = speedModifier ? walker.runSpeed : walker.speed;
            if (axisDirection != 0)
            {
                Vector3 velocity = speed * axisDirection * walker.movementAxis;
                transform.rotation = Quaternion.LookRotation(walker.movementAxis * axisDirection, Vector3.up);
                movementController.Move((velocity + Vector3.down * 4f) * Time.deltaTime);
            }
            else
            {
                movementController.Move(Vector3.down * (Time.deltaTime * 4f));    // TODO: Fall gravity
            }
        }
    }
}
