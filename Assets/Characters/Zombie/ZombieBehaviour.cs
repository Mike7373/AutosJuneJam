using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;


/**
 * Parto da quanto fatto con Athena, solo che invece che utilizzare le "Actions" create da me, rendo ciascuna
 * Action un MonoBehaviour, la coroutine principale diventa l'update di questo behaviour e sfrutto la gestione
 * del ciclo di vita per le callback Start e Exit
 */

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator))]
public class ZombieBehaviour : MonoBehaviour
{
    public float speed = 1.0f;
    public float runSpeed = 6.0f;
    public float walkIdleTime = 3.0f;

    public Vector3 movementAxis = Vector3.right;

    [NonSerialized] public AInputAction<Vector2> moveAction = new();
    [NonSerialized] public AInputAction<float> runModifierAction = new();
    [NonSerialized] public AInputAction<float> punchAction = new();

    [NonSerialized]
    public Animator animator;
    
    [NonSerialized]
    public Rigidbody rigidBody;
    
    Random rng;
    MonoBehaviour currentBehaviour;

    public Component StartAction<T>() where T : MonoBehaviour
    {
        if (currentBehaviour)
        {
            Destroy(currentBehaviour);
        }
        currentBehaviour = gameObject.AddComponent<T>();
        return currentBehaviour;
    }
    
    void Start()
    {
        rng = new Random((uint)DateTime.Now.ToFileTime());
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        StartAction<ZombieIdle>();
        StartCoroutine(Think());
    }
    

    // Il cambiamento di stato per ora è deciso nelle componenti come ZombieIdle, ZombieWalk etc..
    // L'IA può comunque farla da padrona e impostare un behavior.
    // Compito dell'IA è quello di agire sugli eventi e le variabili di stato che governano
    // i vari behaviour come ZombieIdle, ZombieWalk ecc..

    IEnumerator Think()
    {
        MonoBehaviour lastObservedBehaviour = currentBehaviour;
        float whenLastObservedBehaviour = Time.time;
        
        while (true)
        {
            // Ci ricordiamo il tempo dell'ultima azione effettuata
            if (lastObservedBehaviour != currentBehaviour)
            {
                lastObservedBehaviour = currentBehaviour;
                whenLastObservedBehaviour = Time.time;
            }
            
            if (chasingTarget)
            {
                var distance = chasingTarget.position - transform.position;
                if (distance.magnitude <= 1.1f)
                {
                    // PUGNO
                    // Mentre meno il pugno l'IA può cliccare o fare quante cose vuole, non avranno effetto.
                    // E' come se il giocatore continuasse a cliccare ma l'azione deve finire di compiersi.
                    punchAction.Perform(1.0f);
                }
                else
                {
                    // CHASE
                    // La IA ha disposizione i controlli per muovere lo zombie, un Vector2 come se utilizzasse
                    // lo stick di un gamepad. Verso destra si muove verso il movementAxis, verso sinistra al contrario.
                    // Lo zombie vuole muoversi verso la direzione 3d però può muoversi solo lungo il movementAxis,
                    // per cui deve trovare il verso in cui muoversi lungo questo asse che diminuisce la distanza.
                    // Il Dot product di due vettori unitari è il coseno dell'angolo fra i due, il segno mi da
                    // quindi la direzione di movimento.
                    var direction3d = distance.normalized;
                    int axisDirection = Math.Sign(Vector3.Dot(direction3d, movementAxis));
                    moveAction.Perform(new Vector2(axisDirection, 0));
                }
            }
            else if (currentBehaviour is ZombieIdle or ZombieWalk)
            {
                // Vediamo per quanto tempo siamo rimasti in questo stato
                if (Time.time - whenLastObservedBehaviour >= walkIdleTime)
                {
                    int direction = (int) rng.NextUInt(3) - 1;
                    if (direction != 0)
                    {
                        moveAction.Perform(new Vector2(direction, 0));
                    }
                    else
                    {
                        moveAction.Cancel();
                    }
                }
            }
            yield return null;
        }
    }


    Transform chasingTarget;
    void OnTriggerEnter(Collider other)
    {
        chasingTarget = other.transform;
    }

    void OnTriggerExit(Collider other)
    {
        chasingTarget = null;
        moveAction.Cancel();
    }
    
    
    public bool IsGrounded() => groundingColliders.Count > 0;
    HashSet<Collider> groundingColliders = new ();

    void OnCollisionEnter(Collision c)
    {
        // Se collido con qualcosa e la normale del punto di contatto è 
        // verso l'alto, allora lo consideriamo come atterraggio.
        for (int i = 0; i < c.contactCount; i++)
        {
            if (c.GetContact(i).normal.y > 0)
            {
                groundingColliders.Add(c.collider);
                // TODO: Sporco, fallo da un'altra parte, tipo in un handler quando cambia "isGrounded"
                // TODO: Questa gestione del "Grounded" Può andare in una componente.
                //animator.SetBool(AnimatorProperties.IsGrounded, IsGrounded());
                return;
            }
        }
    }

    void OnCollisionExit(Collision c)
    {
        groundingColliders.Remove(c.collider);
        //animator.SetBool(AnimatorProperties.IsGrounded, IsGrounded());
    }
    
}
