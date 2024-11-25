using UnityEngine;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

/**
 *
 * NOTA: Ho dovuto disabilitare nelle preferences della fisica ho "Query Hit Triggers".
 * Non funzionava infatti con lo zombie che ha un grosso collider di allarme trigger intorno,
 * e la OnMouseEnter veniva chiamata anche quando il mouse era lontano dallo zombie, ma dentro la
 * sfera del trigger.
 */
public class HighlitedSelectable : MonoBehaviour
{
    [SerializeField] HighlightSettings settings;
    EventInstance clickSound;
    
    
    // Chiamata dall'editor quando lo script viene creato o i suoi valori modificati
    void OnValidate()
    {

    }

    void OnDestroy()
    {
        clickSound.stop(STOP_MODE.IMMEDIATE);
        clickSound.release();
    }

    void OnMouseEnter()
    {
        settings.material.color = settings.highlightedColor;
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Highlight"));
    }

    void OnMouseExit()
    {
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
    }

    void OnMouseDown()
    {
        settings.material.color = settings.selectedColor;
    }

    void OnMouseUp()
    {
        settings.material.color = settings.highlightedColor;
        clickSound.start();
    }
    
    static void SetLayerRecursively(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        // TODO: Brutale, vedi se è veramente efficiente o se è meglio navigare tutti i figli manualmente
        foreach (var t in gameObject.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = layer;
        }
    }
}
