using System.Collections;
using Characters;
using Input;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AthenaJump : MonoBehaviour
{
    AthenaBehavior player;
    ActionRunner actionRunner;
    Animator animator;

    CharacterController movementController;
    
    CharacterInputAction jumpAction;
    CharacterInputAction moveAction;
    CharacterInputAction runModifierAction;
    EventInstance jumpSound;
    
    void Awake()
    {
        player       = GetComponent<AthenaBehavior>();
        animator     = GetComponent<Animator>();
        actionRunner = GetComponent<ActionRunner>();
        movementController = GetComponent<CharacterController>();
        
        var characterInput = GetComponent<CharacterInput>();
        runModifierAction = characterInput.GetAction("RunModifier");
        moveAction = characterInput.GetAction("Move");
        jumpAction = characterInput.GetAction("Jump");
        
        jumpAction.canceled += EndJumpInput;
        animator.SetBool(AnimatorProperties.IsJumping, true);
        StartCoroutine(JumpCoroutine());
        jumpSound = RuntimeManager.CreateInstance(FMODEvents.instance.jump);
        jumpSound.setVolume(0.5f);
        animator.SetBool(AnimatorProperties.IsGrounded, false);
    }
    
    void Start()
    {
        jumpSound.start();
        jumpSound.release();
    }

    void OnDestroy()
    {
        animator.SetBool(AnimatorProperties.IsJumping, false);
        jumpAction.canceled -= EndJumpInput;
    }

    void EndJumpInput()
    {
        actionRunner.StartAction<AthenaFalling>();
    }

    
    /**
     * Il salto imposta la velocità direttamente, finchè si tiene premuto il tasto di salto questa velocità viene
     * mantenuta. Se viene rilasciato prima, si cade prima. Serve a fare anche i saltini.
     * 
     * TODO: Forse meglio sostituire jumpDistance con jumpTime. YES
     */
    IEnumerator JumpCoroutine()
    {
        float jumpDistance = 0;
        float lastPos = transform.position.y;
        while (jumpDistance < player.jumpRange)
        {
            // In volo mi muovo
            Vector3 velocity = Vector3.zero;
            Vector2 inputValue = moveAction.ReadValue<Vector2>();
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            bool speedModifier = runModifierAction.IsInProgress();
            float speed = speedModifier ? player.runSpeed : player.speed;
            if (axisDirection != 0)
            {
                transform.rotation = Quaternion.LookRotation(player.movementAxis * axisDirection, Vector3.up);
                velocity = speed * axisDirection * player.movementAxis;
            }

            // Finchè sto saltando alzo la mia posizione
            velocity += new Vector3(0, player.jumpSpeed, 0);
            movementController.Move(velocity * Time.deltaTime);

            yield return null;
            jumpDistance += transform.position.y - lastPos;
            lastPos = transform.position.y;
        }
        actionRunner.StartAction<AthenaFalling>();
    }
    
}
