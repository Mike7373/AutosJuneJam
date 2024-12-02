using Characters;
using Characters.Rigging;
using Input;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class AthenaAim : MonoBehaviour
{
    ActionRunner actionRunner;
    CharacterInputAction aimAction;
    CharacterInputAction shootAction;
    Animator animator;
    PistolRig pistolRig;

    Transform pistolControl;
    Pistol pistolPrefab;
    Pistol pistol;
    
    int raycastMask;

    void Awake()
    {
        CoherenceCheck();
        raycastMask = ~(1 << gameObject.layer);

        actionRunner       =  GetComponent<ActionRunner>();
        
        var characterInput =  GetComponent<CharacterInput>();
        aimAction          =  characterInput.GetAction("Aim");
        aimAction.canceled += AimActionCanceled;
        shootAction            = characterInput.GetAction("Fire");
        shootAction.performed += ShootActionPerformed;
        
        pistolPrefab = GetComponent<Shooter>().pistolPrefab;
        pistol = Instantiate(pistolPrefab, transform);
        pistolRig = GetComponent<PistolRig>();
        pistolRig.Bind(pistol);

        animator = GetComponent<Animator>();
        animator.SetBool(AnimatorProperties.IsAiming, true);
    }

    void ShootActionPerformed(object obj)
    {
        animator.SetTrigger(AnimatorProperties.Shoot);
        pistol.Shoot();
    }

    void AimActionCanceled()
    {
        actionRunner.StartAction<AthenaIdle>();
    }
    
    void OnDestroy()
    {
        shootAction.performed -= ShootActionPerformed;
        aimAction.canceled    -= AimActionCanceled;
        animator.SetBool(AnimatorProperties.IsAiming, false);
        Destroy(pistol.gameObject);
    }
    
    void Update()
    {
        // TODO: Prendi la camera da una componente? (La camera va presa dall'Athena Behaviour, o gli viene passata in fase di creazione allAthenaAim, oppure la prende tramite query sulla scena (Dependency injection)
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);

        if (Physics.Raycast(ray, out var hit, 300, raycastMask))
        {
            pistol.AimTo(hit.point);
        }
    }
    
    /**
     * Fa dei check sui reguisiti dello script circa la configurazione del game object e logga dei warning
     * nel caso qualcosa non sia a posto. Utile per debuggare quando le cose non funzionano come dovrebbero.
     */
    void CoherenceCheck()
    {
        if (gameObject.layer == 0)
        {
            Debug.LogWarning($"Lo script {this.GetType().Name} per girare correttamente deve essere su un layer diverso da quello di default, altrimenti il player si può sparare addosso.");
        }
    }
    
}
