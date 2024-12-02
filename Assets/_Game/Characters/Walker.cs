using FMODUnity;
using UnityEngine;

/**
 * Classe contentente i dati per il sistema di movimento attuale, che fa muovere il personaggio avanti e indietro,
 * saltare e cadere.
 */
public class Walker : MonoBehaviour
{
    public float speed          = 3;
    public float runSpeed       = 6;
    public float jumpRange      = 1;
    public float jumpSpeed      = 3;
    
    public EventReference footsteps;
}
