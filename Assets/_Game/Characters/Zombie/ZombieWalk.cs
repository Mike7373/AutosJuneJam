using Characters;
using Characters.Zombie;
using FMOD.Studio;
using FMODUnity;
using Input;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

/**
 *
 * Vedi note su AthenaWalk.
 *
 * L'unica differenza mi sembrano le transazioni su jumpAction e aimAction.
 * TODO: Per introdurre differenze tra i behavior posso fare in modo che lo zombie si possa muovere in qualunque direzione
 * senza essere vincolato ad un piano. Se lascio tutto uguale ragiono su come riusare il codice senza fare copia incolla per
 * cose che si comportano allo stesso modo.
 * 
 */
public class ZombieWalk : MonoBehaviour
{
    Walker walker;
    CharacterController movementController;
    ActionRunner actionRunner;
    
    Vector3 movementDirection;
    
    Animator animator;
    CharacterInputAction punchAction;
    CharacterInputAction runModifierAction;
    CharacterInputAction moveAction;
    
    EventInstance footsteps;

    void Awake()
    {
        walker       = GetComponent<Walker>();
        
        actionRunner = GetComponent<ActionRunner>();
        animator     = GetComponent<Animator>();
        movementController = GetComponent<CharacterController>();
        
        var characterInput = GetComponent<CharacterInput>();
        punchAction = characterInput.GetAction("Punch");
        moveAction = characterInput.GetAction("Move");
        runModifierAction = characterInput.GetAction("RunModifier");
        
        moveAction.canceled         += MoveActionOncanceled;
        punchAction.performed       += PunchActionOnperformed;
        runModifierAction.performed += RunModifierPerformed;
        runModifierAction.canceled  += RunModifierCancelled;
        animator.SetBool(AnimatorProperties.IsMoving, true);
        
        // TODO: Portalo su WalkableComponentData può diventare un Object e basta senza portarsi dietro 
        // l'overload delle routine di un MonoBehavior? Va bene anche uno ScriptableObject?
        footsteps = RuntimeManager.CreateInstance(FMODEvents.instance.footsteps);
        footsteps.setVolume(0.2f);
    }

    void PunchActionOnperformed(object _)
    {
        actionRunner.StartAction<ZombiePunch>();
    }
    void MoveActionOncanceled()
    {
        actionRunner.StartAction<ZombieIdle>();
    }

    
    void RunModifierPerformed(object _)
    {
        animator.SetBool(AnimatorProperties.SpeedModifier, true);
    }

    void RunModifierCancelled()
    {
        animator.SetBool(AnimatorProperties.SpeedModifier, false);
    }
        

    void OnDestroy()
    {
        moveAction.canceled -= MoveActionOncanceled;
        punchAction.performed -= PunchActionOnperformed;
        runModifierAction.performed -= RunModifierPerformed;
        runModifierAction.canceled -= RunModifierCancelled;
        animator.SetBool(AnimatorProperties.IsMoving, false);
        footsteps.stop(STOP_MODE.ALLOWFADEOUT);
    }
    
    void Update()
    {
        if (!movementController.isGrounded)
        {
            actionRunner.StartAction<ZombieFalling>();
        }
        else
        {
            bool speedModifier = runModifierAction.IsInProgress();
            float speed = speedModifier ? walker.runSpeed : walker.speed;
            Vector2 inputValue = moveAction.ReadValue<Vector2>();
            if (inputValue.x != 0)
            {
                int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
                transform.rotation = Quaternion.LookRotation(walker.movementAxis * axisDirection, Vector3.up);

                Vector3 movement = walker.movementAxis * (speed * axisDirection * Time.deltaTime);
                movement.y = -1 * Time.deltaTime;
                movementController.Move(movement);
            }
        }
    }

}
