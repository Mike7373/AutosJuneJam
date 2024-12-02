using Characters;
using Input;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Walker), typeof(CharacterController)),
 RequireComponent(typeof(DeviceCharacterInput), typeof(ActionRunner)),
 RequireComponent(typeof(Animator), typeof(Shooter))]
public class AthenaBehavior : MonoBehaviour
{
    void Start()
    {
        GetComponent<ActionRunner>().StartAction<AthenaIdle>();
    }

}
