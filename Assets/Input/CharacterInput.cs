using System.Collections.Generic;
using UnityEngine;

namespace Input
{

    /**
     *
     * TODO: PER IL MOVIMENTO: https://docs.unity3d.com/ScriptReference/CharacterController.html
     *  Ignora le forze e usa solo la funzione Move. Vincolato però dalle collisioni.
     * 
     * Classe che astrare dal PlayerInput e dalle InputActions.
     *
     * I Behaviour hanno questa componente invece che direttamente il PlayerInput o le action elencate come
     * in ZombieBehaviour.
     *
     * Le componenti fanno
     *     var characterInput = GetComponent<CharacterInput>();
     *     var moveAction = characterInput.actions["Move"];
     *
     *     moveAction.performed ecc..
     *
     * In realtà la componente ha una sottoclasse di CharacterInput, che è PlayerCharacterInput o AICharacterInput.
     * La prima wrappa un PlayerInput.
     *
     * 
     * 
     * E' possibile a runtime sostituire il characterInput di una entità.
     *     
     */
    public abstract class CharacterInput : MonoBehaviour
    {
        public abstract CharacterAction<T> GetAction<T>(string actionName) where T : struct;
    }
    
}