using UnityEngine;
using UnityEngine.Animations.Rigging;

/**
 * Contiene informazioni e metodi per legare l'IK su un modello per una pistola
 * 
 * TODO: Aggiungi un modo per popolare automaticamente i riferimenti, a runtime e da editor.
 * 
 */

public class PistolIKBinder : MonoBehaviour
{
    [field: SerializeField] public TwoBoneIKConstraint rightHandRig {get; private set;}
    [field: SerializeField] public ChainIKConstraint   leftHandRig {get; private set;}
    [field: SerializeField] public MultiAimConstraint  headRig {get; private set;}
    [field: SerializeField] public DampedTransform     pistolDampRig {get; private set;}
    [field: SerializeField] public Transform           pistolPivotAtRest {get; private set;}

    [field: SerializeField] public int handRigIndex {get; private set;}
    [field: SerializeField] public int dampRigIndex {get; private set;}

    // Si può generalizzare la BindTo su un generico BindablePistolData
    public void BindTo(Pistol pistol)
    {
        rightHandRig.data.target = pistol.rightHandIK;
        leftHandRig.data.target = pistol.leftHandIK;
        headRig.data.sourceObjects.Clear();
        headRig.data.sourceObjects.Add(new WeightedTransform(pistol.aimHeadIK, 1));

        pistolDampRig.data.constrainedObject = pistol.transform;
        pistol.phantomPivot = pistolDampRig.data.sourceObject;
        pistol.pivotAtRest = pistolPivotAtRest;
        
    }
}
