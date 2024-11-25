using System.Collections.Generic;
using UnityEngine;


/**
 * TODO: Se uso un character controller la OnCollisionEnter non viene chiamata, posso fare due cose:
 *  1 - Attivo un rigidBody e la gravity solo nell'AthenaFalling.
 *  2 - Scrivo un nuovo GroundChecker che fa il raycast.
 */
public class GroundChecker : MonoBehaviour
{
    HashSet<Collider> groundingColliders = new ();
    
    public bool IsGrounded() => groundingColliders.Count > 0;

    void OnCollisionEnter(Collision c)
    {
        Debug.Log("COLLIDEEE");
        // Se collido con qualcosa e la normale del punto di contatto è 
        // verso l'alto, allora lo consideriamo come atterraggio.
        DebugContacts(c, Color.green);
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
    
    public static void DebugContacts(Collision c, Color color)
    {
        for (int i = 0; i < c.contactCount; i++)
        {
            var contact = c.GetContact(i);
            Debug.DrawRay(contact.point, contact.normal, color, 4);
        }
    }
    
}

