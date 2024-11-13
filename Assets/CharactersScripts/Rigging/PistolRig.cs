using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Characters.Rigging
{

public class PistolRig : MonoBehaviour
{
    public MultiParentConstraint pistolOnGrip;
    public Transform pistolControl;

    public void Bind(PistolV2 pistol)
    {
        pistolOnGrip.data.constrainedObject = pistol.transform;
        pistol.pistolControl = pistolControl;
    }
}


}