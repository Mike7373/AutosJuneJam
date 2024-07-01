using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
    public float alarmDistance = 5.0f;
    public float punchDistance = 2.0f;
    public float attackDelay = 0.6f;

    public Vector3 movementAxis = Vector3.right;

    [NonSerialized] public AInputAction<Vector2> moveAction = new();
    [NonSerialized] public AInputAction<bool> runModifierAction = new();
    [NonSerialized] public AInputAction<bool> punchAction = new();

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
            DestroyImmediate(currentBehaviour);
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
        while (true)
        {


            /*switch (currentBehaviour)
            {
                case ZombieIdle:
                    if (rng.NextBool())
                    {
                        // Decide di muoversi
                        Debug.Log("Lo zombie preme D!");
                        KeyboardState kb = new KeyboardState(Key.D);
                        InputSystem.QueueStateEvent(zombieInputDevice, kb);
                    }
                    else
                    {
                        Debug.Log("Lo zombie non fa un cazzo");
                    }

                    break;
                case ZombieWalk:
                    if (rng.NextBool())
                    {
                        Debug.Log("Lo zombie rilascia tutti i tasti");
                        // Decide di fermarsi
                        KeyboardState kb = new KeyboardState();
                        InputSystem.QueueStateEvent(zombieInputDevice, kb);
                    }
                    else
                    {
                        Debug.Log("Lo zombie continua a premere D!");
                    }

                    break;
            }*/

            yield return new WaitForSeconds(1.0f);

        }
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
