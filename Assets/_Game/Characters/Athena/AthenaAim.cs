using Characters;
using Characters.Rigging;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: Prendi la camera da una componente? (La camera va presa dall'Athena Behaviour, o gli viene passata in fase di creazione allAthenaAim, oppure la prende tramite query sulla scena (Dependency injection)

public class AthenaAim : MonoBehaviour
{
    ActionRunner actionRunner;
    CharacterInputAction aimAction;
    CharacterInputAction lookAction;
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
        aimAction.canceled += AimCanceled;
        shootAction            = characterInput.GetAction("Fire");
        shootAction.performed += ShootPerformed;
        lookAction = characterInput.GetAction("Look");

        pistolPrefab = GetComponent<Shooter>().pistolPrefab;
        pistol       = Instantiate(pistolPrefab, transform);
        pistolRig    = GetComponent<PistolRig>();
        pistolRig.Bind(pistol);

        animator = GetComponent<Animator>();
        animator.SetBool(AnimatorProperties.IsAiming, true);

        Cursor.visible = false;
    }
    
    /**
     * Dapprima faccio la AimTo laddove è puntato il cursore, successivamente sposto la mira
     * ascoltando le mouse move. In questo modo il movimento dell'arma risulta naturale.
     * 
     */
    void Start()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);

        // TODO: Che succede se non entro in questo if?
        if (Physics.Raycast(ray, out var hit, 300, raycastMask))
        {
            pistol.AimTo(hit.point);
            lookAction.performed += LookPerformed;
        }
    }
    
    void OnDestroy()
    {
        shootAction.performed -= ShootPerformed;
        aimAction.canceled    -= AimCanceled;
        lookAction.performed  -= LookPerformed;
        animator.SetBool(AnimatorProperties.IsAiming, false);
        Destroy(pistol.gameObject);
        Cursor.visible = true;
    }
    
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);
    }
    
    void LookPerformed(object obj)
    {
        var mouseMovement = (Vector2) obj;
        // Scambio gli assi per far corrispondere il movimento del mouse a pitch e yaw
        pistol.RotateAim(new Vector2(-mouseMovement.y, mouseMovement.x));
    }

    void ShootPerformed(object obj)
    {
        animator.SetTrigger(AnimatorProperties.Shoot);
        pistol.Shoot();
    }

    void AimCanceled()
    {
        actionRunner.StartAction<AthenaIdle>();
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
