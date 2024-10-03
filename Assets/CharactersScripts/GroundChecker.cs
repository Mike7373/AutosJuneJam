using System.Collections.Generic;
using UnityEngine;

    
public class GroundChecker : MonoBehaviour
{
    HashSet<Collider> groundingColliders = new ();
    
    public bool IsGrounded() => groundingColliders.Count > 0;

    void OnCollisionEnter(Collision c)
    {
        // Se collido con qualcosa e la normale del punto di contatto è 
        // verso l'alto, allora lo consideriamo come atterraggio.
        for (int i = 0; i < c.contactCount; i++)
        {
            if (c.GetContact(i).normal.y > 0)
            {
                groundingColliders.Add(c.collider);
                return;
            }
        }
    }

    void OnCollisionExit(Collision c)
    {
        groundingColliders.Remove(c.collider);
    }
    
}

