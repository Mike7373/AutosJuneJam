using Characters;
using Input;
using UnityEngine;

public class AthenaIdle : MonoBehaviour
{
    ActionRunner actionRunner;
    GroundChecker groundChecker;

    NavigationManager navigationManager;

    CharacterInputAction jumpAction;
    CharacterInputAction punchAction;
    CharacterInputAction moveAction;
    CharacterInputAction aimAction;

    void Awake()
    {
        actionRunner = GetComponent<ActionRunner>();
        groundChecker = GetComponent<GroundChecker>();
         

        var characterInput = GetComponent<CharacterInput>();
        jumpAction = characterInput.GetAction("Jump");
        punchAction = characterInput.GetAction("Punch");
        moveAction = characterInput.GetAction("Move");
        aimAction = characterInput.GetAction("Aim");
        
        // Qui devo prima registrare gli handlers, perchè la coroutine mi fa un passaggio di stato 
        // nello stesso frame e non capisco perchè, la Stop non deregistra gli handlers
        // Forse con la versione a componenti con Start()  e Update() si risolve.
        jumpAction.performed += JumpActionOnperformed;
        punchAction.performed += PunchActionOnperformed;
        aimAction.performed += AimActionPerformed;
        
        navigationManager = FindAnyObjectByType<NavigationManager>();
        if (navigationManager != null)
        {
            // TODO: Togli check != null. (Oppure logga un warning)
            // l'ho messo ora per usare velocemente athena idle anche altrove. 
            navigationManager.onNavigate += OnNavigate;    
        }
        
    }

    void AimActionPerformed(object obj)
    {
        actionRunner.StartAction<AthenaAim>();
    }

    void OnDestroy()
    {
        jumpAction.performed -= JumpActionOnperformed;
        punchAction.performed -= PunchActionOnperformed;
        aimAction.performed -= AimActionPerformed;

        if (navigationManager != null)
        {
            // TODO: Togli check != null
            navigationManager.onNavigate -= OnNavigate;
        }
    }

    
    void PunchActionOnperformed(object f)
    {
        actionRunner.StartAction<AthenaPunch>();
    }
    
    
    public void JumpActionOnperformed(object f)
    {
        actionRunner.StartAction<AthenaJump>();
    }

    
    void FixedUpdate()
    {
        if (!groundChecker.IsGrounded())
        {
            actionRunner.StartAction<AthenaFalling>();
        }
        else
        {
            if (moveAction.IsInProgress())
            {
                actionRunner.StartAction<AthenaWalk>();
            }
        }
    }

    private void OnNavigate(Navigable navigable)
    {
        AthenaNavigation nextState = (AthenaNavigation) actionRunner.StartAction<AthenaNavigation>();
        nextState.navigable = navigable;
        nextState.startPosition = transform.position;
    }

}
