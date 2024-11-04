using System;
using Characters;
using FMODUnity;
using Input;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;


// FEATURES: (Le segno per spiegare agli altri come sono implementate)
// Angolo di rotazione delle braccia\torso prima di far ruotare il giocatore.
// La testa guarda la pistola con l'IK, in modo da sembrare che guardi nel mirino.
public class AthenaAim : MonoBehaviour
{

    RigBuilder ikRigBuilder;
    ActionRunner actionRunner;
    Animator animator;
    CharacterInputAction aimAction;
    CharacterInputAction lookAction;
    Transform pistolIKHandle;
    Transform pistolPivot;

    float pistolMaxAngle = 20;             // Angolo massimo delle braccia rispetto al corpo oltre al quale il giocatore ruota 
    
    void Start()
    {
        actionRunner   = GetComponent<ActionRunner>();
        ikRigBuilder   = GetComponent<RigBuilder>();
        pistolIKHandle = GetComponent<AthenaBehavior>().pistolIKHandle;
        pistolPivot = GetComponent<AthenaBehavior>().pistolRotatingPivot;
        
        var characterInput = GetComponent<CharacterInput>();
        aimAction  = characterInput.GetAction("Aim");
        aimAction.canceled += AimActionCanceled;
        
        lookAction = characterInput.GetAction("Look");
        
        animator = GetComponent<Animator>();
        animator.SetBool(AnimatorProperties.IsAiming, true);
        //ikRigBuilder.layers[(int) AthenaBehavior.RigLayers.Pistol].active = true;
    }
    
    void OnAnimatorIK(int layerIndex)
    {
        
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
        animator.SetIKPosition(AvatarIKGoal.RightHand,pistolIKHandle.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand,pistolIKHandle.rotation);
        
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);  
        animator.SetIKPosition(AvatarIKGoal.LeftHand,pistolIKHandle.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand,pistolIKHandle.rotation);
        
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(pistolIKHandle.position);
    }

    void AimActionCanceled()
    {
        animator.SetBool(AnimatorProperties.IsAiming, false);
        aimAction.canceled   -= AimActionCanceled;
        actionRunner.StartAction<AthenaIdle>();
    }

    void OnDestroy()
    {
        animator.SetBool(AnimatorProperties.IsAiming, false);
        //ikRigBuilder.layers[(int)AthenaBehavior.RigLayers.Pistol].active = false;
    }

    float lastAngleY;
    
    void Update()
    {
        // TODO: Prendi la camera da una componente?
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);
        
        
        if (Physics.Raycast(ray, out var hit))
        {
            // ========== GESTIONE ROTAZIONE ============
            // La pistola può ruotare al massimo di "pistolMaxAngle" gradi sulla Y.
            // Se la rotazione eccede, ruotiamo il personaggio sulla Y.
            // Per stabilire il verso della rotazione consideriamo il precedente angleY
            pistolPivot.LookAt(hit.point);
            
            float angleY = pistolPivot.localRotation.eulerAngles.y;
            var rightBound = pistolMaxAngle;
            var leftBound  = 360-pistolMaxAngle;
            if (angleY > rightBound && angleY < leftBound)
            {
                if (lastAngleY >= 0 && lastAngleY <= pistolMaxAngle + 0.001)
                {
                    // ES: Prima stavo a 15° ora sono a 30°, il margine destro è 20°. ruoto il pivot di 10° in senso antiorario e il player in senso orario
                    var diff = angleY-rightBound;
                    pistolPivot.Rotate(new Vector3(0, -diff,0));
                    transform.Rotate(new Vector3(0, diff, 0));
                }
                else
                {
                    // ES: Prima stavo a 350° e ora sono a 340°, ruoto il pivot in senso orario e il player in senso antiorario
                    var diff = leftBound-angleY;
                    pistolPivot.Rotate(new Vector3(0, diff,0));
                    transform.Rotate(new Vector3(0, -diff, 0));
                }
            }
            lastAngleY = pistolPivot.localRotation.eulerAngles.y;
            
            
            // ============== GESTIONE FUOCO ===============
            /*
            var pistolRay = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(pistolRay, out hit))
            {
                luceMirino.SetActive(true);
                luceMirino.transform.position = hit.point-ray.direction*0.1f;
                luceMirino.transform.LookAt(hit.point);
            }*/
        }
    }
    
}
