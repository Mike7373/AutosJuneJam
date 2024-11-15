using System;
using Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// A seconda del tasto che viene premuto:
//  1 - Situazione normale, Athena controllata da dispositivo e lo Zombie dalla IA
//  2 - Athena senza input, Zombie con input da dispositivo
//  3 - Athena con input IA, Zombie con input da dispositivo

// Al momento distruggo le vecchie componenti e istanzio quelle che fanno al caso.

public class ZombieAthenaSwitcher : MonoBehaviour
{
    public GameObject zombie;
    public GameObject athena;

    Action<int> a;

    void Update()
    {

        a += i => Debug.Log(i);

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            AthenaInputDeviceZombieInputAI();
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            AthenaNoInputZombieInputDevice();

        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            AthenaInputAIZombieInputDevice();
        }
    }

    // Imposto ad Athena il NoOpCharacterInput e sullo zombie il DeviceCharacterInput.
    void AthenaNoInputZombieInputDevice()
    {
        Debug.Log(nameof(AthenaNoInputZombieInputDevice));

        var athenaInput = athena.GetComponent<CharacterInput>();
        Destroy(athenaInput);
        var noOpInput = athena.AddComponent<NoOpCharacterInput>();
        noOpInput.Acquire(athenaInput);

        var zombieInput = zombie.GetComponent<CharacterInput>();
        Destroy(zombieInput);
        var deviceInput = zombieInput.AddComponent<DeviceCharacterInput>();
        deviceInput.Acquire(zombieInput);

        var zombieIA = zombie.GetComponent<ZombieIA>();
        if (zombieIA)
        {
            Destroy(zombieIA);
        }
    }

    void AthenaInputDeviceZombieInputAI()
    {
        Debug.Log(nameof(AthenaInputDeviceZombieInputAI));

        var athenaInput = athena.GetComponent<CharacterInput>();
        Destroy(athenaInput);
        var deviceInput = athena.AddComponent<DeviceCharacterInput>();
        deviceInput.Acquire(athenaInput);

        var zombieInput = zombie.GetComponent<CharacterInput>();
        Destroy(zombieInput);
        var aiInput = zombieInput.AddComponent<AICharacterInput>();
        aiInput.Acquire(zombieInput);

        var zombieIA = zombie.GetComponent<ZombieIA>();
        if (!zombieIA)
        {
            zombie.AddComponent<ZombieIA>();
        }
    }


    void AthenaInputAIZombieInputDevice()
    {
        Debug.Log(nameof(AthenaInputDeviceZombieInputAI));
        
        var athenaInput = athena.GetComponent<CharacterInput>();
        Destroy(athenaInput);
        var aiInput = athena.AddComponent<AICharacterInput>();
        aiInput.Acquire(athenaInput);
        
        var zombieIA = athena.GetComponent<ZombieIA>();
        if (!zombieIA)
        {
            athena.AddComponent<ZombieIA>();
        }
        
        var zombieInput = zombie.GetComponent<CharacterInput>();
        Destroy(zombieInput);
        var deviceInput = zombieInput.AddComponent<DeviceCharacterInput>();
        deviceInput.Acquire(zombieInput);

        var zombieZombieIA = zombie.GetComponent<ZombieIA>();
        if (zombieZombieIA)
        {
            Destroy(zombieZombieIA);
        }


    }
}
