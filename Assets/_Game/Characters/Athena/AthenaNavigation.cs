using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR.Haptics;

public class AthenaNavigation : MonoBehaviour
{
    public Navigable navigable;
    public Vector3 startPosition;
    ActionRunner actionRunner;
    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update

    void Awake()
    {
        actionRunner = GetComponent<ActionRunner>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        //navigationPlayer = GetComponent<NavigationPlayer>();
        //navigationPlayer.enabled = true;
        //navigationPlayer.navigable = navigable.GetComponent<Transform>();
    }
    void Start()
    {
        StartCoroutine(Navigate());
    }

    public void OnDestroy()
    {

    }

    IEnumerator Navigate()
    {
        navMeshAgent.destination = navigable.transform.position;
        //Debug.Log(navMeshAgent.SetDestination(navigable.transform.position));
        //yield return WaitForReach();
        yield return new WaitForSeconds(5.0f);
        navMeshAgent.destination = startPosition;
        //yield return WaitForReach();
        yield return new WaitForSeconds(5.0f);
        actionRunner.StartAction<AthenaIdle>();
        navMeshAgent.enabled = false;
    }

    IEnumerator WaitForReach()
    {
        bool condition = (navMeshAgent.remainingDistance!=Mathf.Infinity && 
                navMeshAgent.pathStatus==NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance==0);

        Debug.Log(navMeshAgent.remainingDistance);
        Debug.Log(navMeshAgent.pathStatus);
        while (!(condition)) {
            Debug.Log("Dentro");
            yield return null;
        }
    }
}
