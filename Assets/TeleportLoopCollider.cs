using System.Collections;
using UnityEngine;

public class TeleportLoopCollider : MonoBehaviour
{
    [SerializeField]
    Transform target;

    void OnTriggerEnter(Collider other)
    {
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
