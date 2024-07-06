using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Input
{
    public class DeviceCharacterInput : CharacterInput
    {
        PlayerInput playerInput;
        Dictionary<string, object> actions = new();

        void Awake()
        {
            playerInput = FindObjectOfType<PlayerInput>();
        }
        
        public override CharacterAction<T> GetAction<T>(string actionName)
        {
            if (actions.TryGetValue(actionName, out var action))
            {
                return (DeviceInputAction<T>) action;
            }
            else
            {
                var newAction = new DeviceInputAction<T>(playerInput.actions[actionName]);
                actions.Add(actionName, newAction);
                return newAction;
            }
        }
    }
}