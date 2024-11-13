using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Characters.Rigging
{

public class PistolRig : MonoBehaviour
{
    public MultiParentConstraint pistolOnGrip;
    public Transform pistolControl;

    public void Bind(Pistol pistol)
    {
        pistolOnGrip.data.constrainedObject = pistol.transform;
        pistol.pistolControl = pistolControl;
        GetComponentInParent<RigBuilder>().Build();
    }
}


}