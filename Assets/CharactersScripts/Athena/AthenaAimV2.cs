using Characters;
using Characters.Rigging;
using Input;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class AthenaAimV2 : MonoBehaviour
{
    
    ActionRunner actionRunner;
    CharacterInputAction aimAction;
    Animator animator;
    PistolRig pistolRig;

    Transform pistolControl;
    PistolV2 pistolPrefab;

    PistolV2 pistol;
    
    int raycastMask;

    void Awake()
    {
        CoherenceCheck();
        raycastMask = ~(1 << gameObject.layer);

        actionRunner       =  GetComponent<ActionRunner>();
        
        var characterInput =  GetComponent<CharacterInput>();
        aimAction          =  characterInput.GetAction("Aim");
        aimAction.canceled += AimActionCanceled;
        
        // TODO: pistolPrefab unico riferimento ad AthenaBehavior, come lo tolgo?
        pistolPrefab = GetComponent<AthenaBehavior>().pistolPrefab;
        pistol = Instantiate(pistolPrefab, transform);
        
        pistolRig = GetComponent<PistolRig>();
        pistolRig.Bind(pistol);
        GetComponent<RigBuilder>().Build();
        animator = GetComponent<Animator>();
        animator.SetBool(AnimatorProperties.IsAiming, true);
    }

    void AimActionCanceled()
    {
        actionRunner.StartAction<AthenaIdle>();
    }
    

    void OnDestroy()
    {
        aimAction.canceled  -= AimActionCanceled;
        animator.SetBool(AnimatorProperties.IsAiming, false);
        Destroy(pistol.gameObject);
    }
    
    void Update()
    {
        // TODO: Prendi la camera da una componente? (La camera va presa dall'Athena Behaviour, o gli viene passata in fase di creazione allAthenaAim (Dependency injection)
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
            Debug.LogWarning($"Lo script {this.GetType().Name} per girare correttamente deve essere su un layer diverso da  quello di default.");
        }
    }
    
}
