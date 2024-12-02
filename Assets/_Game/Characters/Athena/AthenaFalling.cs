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

    float fallSpeed = 4;
    
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
            if (inputValue.x != 0)
            {
                bool speedModifier = runModifierAction.IsInProgress();
                float speed        = speedModifier ? walker.runSpeed : walker.speed;
                int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
                transform.localRotation = Quaternion.LookRotation(Vector3.forward*axisDirection, Vector3.up);
                Vector3 movement = transform.forward * speed + Vector3.down * fallSpeed;
                movementController.Move(movement * Time.deltaTime);
            }
            else
            {
                movementController.Move(Vector3.down * (Time.deltaTime * fallSpeed));    // TODO: Fall gravity
            }
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
