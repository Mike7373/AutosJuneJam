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
     * TODO: Incapsulamento. (eeehh??)
     * TODO: Dictonary<int, CharacterInputAction> cioè con gli hash.
     * 
     */
    public abstract class CharacterInput : MonoBehaviour
    {
        public Dictionary<string, CharacterInputAction> actions = new();

        public abstract InputBinder GetInputBinder(string actionName);

        public CharacterInputAction GetAction(string actionName)
        {
            if (actions.TryGetValue(actionName, out var action))
            {
                return action;
            }
            else
            {
                var noOpBinder = GetInputBinder(actionName);
                var newAction = new CharacterInputAction(noOpBinder);
                actions.Add(actionName, newAction);
                return newAction;
            }
        }

        /**
         * Prende possesso delle azioni del CharacterInput argomento, utilizzando l'InputBinder.
         *
         * TODO: Spegnere l'altro input?
         */
        public void Acquire(CharacterInput other)
        {
            foreach (var (actionName, a) in other.actions)
            {
                a.Bind(GetInputBinder(actionName));
                actions[actionName] = a;
            }
        }
    }
    
}