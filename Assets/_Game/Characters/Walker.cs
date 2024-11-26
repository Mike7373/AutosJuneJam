using UnityEngine;

/**
 * Classe contentente i dati per il sistema di movimento attuale, che fa muovere su di un piano, saltare e cadere.
 */
public class Walker : MonoBehaviour
{
    public Vector3 movementAxis = Vector3.right;
    public float speed          = 3;
    public float runSpeed       = 6;
    public float jumpRange      = 1;
    public float jumpSpeed      = 3;
}
