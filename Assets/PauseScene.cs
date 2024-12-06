using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 *
 * Premendo sulla tastiera "Pause", la view del gioco si mette in pausa. Utile quando si vuole bloccare la scena e
 * analizzare gli oggetti creati.
 * 
 */
public class PauseScene : MonoBehaviour
{
    
    void Update()
    {
        if (Keyboard.current.pauseKey.wasPressedThisFrame)
        {
            EditorApplication.isPaused = true;
        }
    }
}
