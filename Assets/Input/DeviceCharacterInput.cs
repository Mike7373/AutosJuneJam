using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{ 
    
/**
 * Costruisce e gestisce azioni bindate sul DeviceInputBinder
 * 
 */
public class DeviceCharacterInput : CharacterInput
{
    PlayerInput playerInput;

    void Awake()
    {
        Debug.Log("Starting device character input");
        playerInput = FindObjectOfType<PlayerInput>();
    }

    public override InputBinder GetInputBinder(string actionName)
    {
        return new DeviceInputBinder(playerInput.actions[actionName]);
    }

    
}

}