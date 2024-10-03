using System.Collections;
using UnityEngine;

public class TeleportLoopCollider : MonoBehaviour
{
    [SerializeField]
    Transform target;

    void OnTriggerEnter(Collider other)
    {
        // BUG: Quando avviene il teletrasporto loop vedo uno scattino, provo a farlo nella fixedUpdate ma non
        // si risolve. Stranamente avviene solo se il movimento è tramite tastiera, se traslo tramite script
        // il personaggio, il teletrasporto loop avviene in maniera morbida e non si nota.
        StartCoroutine(TeleportToTarget(other.attachedRigidbody));
        //Vector3 dist = other.attachedRigidbody.position - transform.position;
        //other.attachedRigidbody.position = (target.transform.position + dist);
    }

    IEnumerator TeleportToTarget(Rigidbody other)
    {
        yield return new WaitForFixedUpdate();
        
        Vector3 dist = other.position - transform.position;
        Debug.Log($"Line position: {transform.position}\nPlayer position: {other.transform.position}\nDist: {dist}");
        other.position = target.transform.position + dist;
    }
}
