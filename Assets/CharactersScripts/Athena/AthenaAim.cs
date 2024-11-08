using Characters;
using Input;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;


// FEATURES: (Le segno per spiegare agli altri come sono implementate)
// Angolo di rotazione delle braccia\torso prima di far ruotare il giocatore.
// La testa guarda la pistola con l'IK, in modo da sembrare che guardi nel mirino.

// Come faccio a dare la stessa capacità ad una IA? Es: uno Zombie con la pistola in mano punta verso di me.
// Una IA fa mirare il suo personaggio.

// In questo caso la IA non ha a che fare con il mouse, non muove il mouse sullo schermo come noi, ma mira direttamente
// agli oggetti in scena.

// Potrei dividere questo script in due:
//   - AthenaAim.cs legge il mouse, fa il raycast e vede dove colpiamo.
//                  successivamente chiama Pistol.AimTo(target)
//   - PistolAim.cs 
//          AimTo()
//              Ruota il pistolPivot e la transform del giocatore
//
// La IA in questo modo può chiamare AimTo direttamente su di un target\posizione, o secondo una sua logica.

//
// Quando caccio la pistola:
//      Setto PistolAimRig.Hand_L.target = Pistol.LeftHandIK 
//      Setto PistolAimRig.Hand_R.target = Pistol.RightHandIK
//      Setto PistolAimRig.Head.target   = Pistol.transform (Creare poi una transform apposita per dirigere lo sguardo verso il mirino) 
//
//      Setto PistolDampRig.PistolDamp.constrained = Pistol.transform (Il pistolPivot diventa la pistol e il model è come figlio e spostato in avanti)
//      Setto Pistol.phantomPivot = PistolDampRig.PistolDamp.source
//      Setto Pistol.restingPivot = PistolDampRig.PistolDamp.pistolRestingPivot (TODO)<

//      Attivo Il rig PistolAim
//      Attivo il rig pistolDamp
//
// Modelli diversi hanno lunghezza di braccia diverse, il phantomPivot è sul rig ed è personalizzabile per modello.

// Quando reinfodero la pistola:
//  Il phantomPivot ruota in basso  
//      phantomPivot = restingPivot; (Oppure phantomPivot.localRotation = Quaternion.LookRotation(Vector3.down)
//
//  Disattivo Rig PistolAim
//  Disattivo Rig pistolDamp

    

public class AthenaAim : MonoBehaviour
{

    RigBuilder ikRigBuilder;
    ActionRunner actionRunner;
    Animator animator;
    CharacterInputAction aimAction;
    Transform pistolShadowPivot;

    float pistolMaxAngle = 5;             // Angolo massimo delle braccia rispetto al corpo oltre al quale il giocatore ruota (TODO: Vedere dove deve finire questa proprietà) 
    int raycastMask;
    
    
    void Start()
    {
        raycastMask = ~(1 << gameObject.layer);       
        
        actionRunner   = GetComponent<ActionRunner>();
        ikRigBuilder   = GetComponent<RigBuilder>();
        pistolShadowPivot = GetComponent<AthenaBehavior>().pistolPivot;
        
        var characterInput = GetComponent<CharacterInput>();
        aimAction  = characterInput.GetAction("Aim");
        aimAction.canceled += AimActionCanceled;
        
        // TODO: Istanzia\Attiva la pistola
        // Pistol p = Instantiate(pistolPrefab, transform)
        // p.Configure(pistolShadowPivot, transform)
        
            // Il rig lo attiva l'arma, perchè abbiamo un rig diverso a seconda dell'arma.
            // Lo zombie potrebbe avere dei rig diversi rispetto a quelli del player,
            // come fa la pistola a sapere quale layer attivare?
            // Bisogna poi configurare anche i rig, il source object rig della testa deve puntare
            // alla pistola
            // Programma quindi anche un nemico che ci prova a sparare
            
            ikRigBuilder.layers[(int) AthenaBehavior.RigLayers.Pistol].active = true;
            
        animator = GetComponent<Animator>();
        animator.SetBool(AnimatorProperties.IsAiming, true);
    }
    

    void AimActionCanceled()
    {
        pistolShadowPivot.localRotation = Quaternion.LookRotation(Vector3.down);
        actionRunner.StartAction<AthenaIdle>();
    }
    

    void OnDestroy()
    {
        aimAction.canceled   -= AimActionCanceled;
        animator.SetBool(AnimatorProperties.IsAiming, false);
        ikRigBuilder.layers[(int)AthenaBehavior.RigLayers.Pistol].active = false;
    }

    float lastAngleY;
    
    void Update()
    {
        // L'atto del mirare non voglio che sia inscritto nell'"AthenaAim" aLtrimenti per altri personaggi\armi come faccio?
        // Per mirare ho bisogno di:
        // Rig Mani
        // Rig Damping
        // Pivot e pivot dampened
        // La transform del giocatore, perchè ruota anche lui
        // Riesco a fare un generico "PistolAim?"
        
        
        // TODO: Prendi la camera da una componente? (La camera va presa dall'Athena Behaviour, o gli viene passata in fase di creazione allAthenaAim (Dependency injection)
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);
        
        // TODO: 
        /*if (Physics.Raycast(ray, out var hit, 300, raycastMask))
        {
            pistol.AimTo(hit);              
        }
        */
        if (Physics.Raycast(ray, out var hit, 300, raycastMask))
        {
            // ========== GESTIONE ROTAZIONE ============
            // La pistola può ruotare al massimo di "pistolMaxAngle" gradi sulla Y.
            // Se la rotazione eccede, ruotiamo il personaggio sulla Y.
            // Per stabilire il verso della rotazione consideriamo il precedente angleY
            pistolShadowPivot.LookAt(hit.point);
            
            float angleY = pistolShadowPivot.localRotation.eulerAngles.y;
            var rightBound = pistolMaxAngle;
            var leftBound  = 360-pistolMaxAngle;
            if (angleY > rightBound && angleY < leftBound)
            {
                if (lastAngleY >= 0 && lastAngleY <= pistolMaxAngle + 0.001)
                {
                    // ES: Prima stavo a 15° ora sono a 30°, il margine destro è 20°. ruoto il pivot di 10° in senso antiorario e il player in senso orario
                    var diff = angleY-rightBound;
                    pistolShadowPivot.Rotate(new Vector3(0, -diff,0));
                    transform.Rotate(new Vector3(0, diff, 0));
                }
                else
                {
                    // ES: Prima stavo a 350° e ora sono a 340°, ruoto il pivot in senso orario e il player in senso antiorario
                    var diff = leftBound-angleY;
                    pistolShadowPivot.Rotate(new Vector3(0, diff,0));
                    transform.Rotate(new Vector3(0, -diff, 0));
                }
            }
            lastAngleY = pistolShadowPivot.localRotation.eulerAngles.y;
            
            
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
