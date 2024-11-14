using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Vector3 distance;

    void Update()
    {
        transform.position = target.position + distance;
        transform.LookAt(target);
    }
}
