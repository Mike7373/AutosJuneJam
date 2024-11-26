using System.ComponentModel;
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
 * Note:
 * Il personaggio deve essere attaccato al suolo quando cammina, per non levitare e per scendere le scale, introduciamo
 * un leggero movimento verso il basso, in questo modo la isGrounded del movementController funziona ad ogni frame.
 *  
 * TODO:
 *  Vorrei essere certo di vincolare il movimento lungo una direzione e su di un piano. (Ad esempio Z=0)
 *  Errori di approssimazione dei float o risoluzioni di collisioni nel character controller possono spostare
 *  il personaggio dal piano in cui il movimento del giocatore deve ssere vincolato.
 *  Se ci muoviamo lungo gli  assi x, y e z è facile perchè dopo il movimento posso reimpostare direttamente l'asse
 *  scelto al valore desiderato. Ma se mi muovo lungo una direzione arbitraria, come vaccio a mantenerla?
 *  Potrei provare con qualcosa del genere:
 *      character.move(dist)
 *      character.position = project(character.position, movement_plane)
 *  Cioè proietto la posizione del giocatore sul piano.
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
        
        // TODO: Portalo su WalkableComponentData, può diventare un Object e basta senza portarsi dietro 
        // l'overload delle routine di un MonoBehavior e della Transform?
        footsteps = RuntimeManager.CreateInstance(FMODEvents.instance.footsteps);
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
        // Corsa
        bool speedModifier = runModifierAction.IsInProgress();
        animator.SetBool(AnimatorProperties.SpeedModifier, speedModifier);
        float speed = speedModifier ? walker.runSpeed : walker.speed;
        Vector2 inputValue = moveAction.ReadValue<Vector2>();
        if (inputValue.x != 0)
        {
            int axisDirection  = inputValue.x > 0 ? 1 : inputValue.x < 0 ? -1 : 0;
            transform.rotation = Quaternion.LookRotation(walker.movementAxis * axisDirection, Vector3.up);

            Vector3 movement = walker.movementAxis * (speed * axisDirection * Time.deltaTime);
            movement.y = -1;
            movementController.Move(movement);
        }
        if (!movementController.isGrounded)
        {
            actionRunner.StartAction<AthenaFalling>();
        }
    }

}
