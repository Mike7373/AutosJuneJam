using Characters;
using Input;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

/**
 *
 * Behavior di movimento lungo un'asse. (Collegato al resto dei Behavior di Athena)
 * 
 * NOTE:
 * - Il personaggio deve essere attaccato al suolo quando cammina, per non levitare e per scendere le scale, introduciamo
 * un leggero movimento verso il basso, in questo modo la isGrounded del movementController funziona ad ogni frame.
 *
 * - Il movimento avviene lungo l'asse "forward" dell'oggetto e il movimento è vincolato al piano Z Y con la X fissata a zero.
 *   Per muoversi in qualunque direzione, il game object viene messo come figlio di una transform che ne indica la direzione di movimento.
 *   In questo modo vincolare il movimento sul piano è semplice perchè basta muoversi sulla forward e impostare la X a zero.
*/
public class AthenaWalk : MonoBehaviour
{
    Walker walker;
    CharacterController movementController;
    Animator animator;
    ActionRunner actionRunner;

    CharacterInputAction jumpAction;
    CharacterInputAction punchAction;
    CharacterInputAction runModifierAction;
    CharacterInputAction moveAction;
    CharacterInputAction aimAction;
    
    EventInstance footsteps;
    
    void Awake()
    {
        walker       = GetComponent<Walker>();
        actionRunner = GetComponent<ActionRunner>();
        animator     = GetComponent<Animator>();
        movementController = GetComponent<CharacterController>();


        var characterInput = GetComponent<CharacterInput>();
        jumpAction = characterInput.GetAction("Jump");
        punchAction = characterInput.GetAction("Punch");
        moveAction = characterInput.GetAction("Move");
        runModifierAction = characterInput.GetAction("RunModifier");
        aimAction = characterInput.GetAction("Aim");
        
        moveAction.canceled += MoveActionOncanceled;
        jumpAction.performed += JumpActionOnperformed;
        punchAction.performed += PunchActionOnperformed;
        aimAction.performed += AimActionPerformed;
        
        animator.SetBool(AnimatorProperties.IsMoving, true);
        
        footsteps = RuntimeManager.CreateInstance(walker.footsteps);
        footsteps.setVolume(0.2f);
    }

    void Start()
    {    
        footsteps.start();
        footsteps.release();
    }

    void OnDestroy()
    {
        moveAction.canceled -= MoveActionOncanceled;
        jumpAction.performed -= JumpActionOnperformed;
        punchAction.performed -= PunchActionOnperformed;
        aimAction.performed -= AimActionPerformed;
        animator.SetBool(AnimatorProperties.IsMoving, false);
        footsteps.stop(STOP_MODE.ALLOWFADEOUT);
    }

    void AimActionPerformed(object obj)
    {
        actionRunner.StartAction<AthenaAim>();
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
    
    void Update()
    {
        bool speedModifier = runModifierAction.IsInProgress();
        animator.SetBool(AnimatorProperties.SpeedModifier, speedModifier);
        Vector2 inputValue = moveAction.ReadValue<Vector2>();
        if (inputValue.x != 0)
        {
            float speed = speedModifier ? walker.runSpeed : walker.speed;
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            transform.localRotation = Quaternion.LookRotation(Vector3.forward*axisDirection, Vector3.up);
            Vector3 movement = transform.forward * (speed * Time.deltaTime);
            movement.y = -1;
            movementController.Move(movement);
            
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
        if (!movementController.isGrounded)
        {
            actionRunner.StartAction<AthenaFalling>();
        }
    }

}
