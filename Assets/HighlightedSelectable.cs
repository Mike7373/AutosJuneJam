using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FMOD.Studio;

/**
 *
 *  Fa la sovrailluminazione (highlight) di un oggetto, spostandolo nel layer "Highlight"
 *  Una RenderFeature aggiuntiva si preoccupa di fare il disegno con il materiale unlit di override.
 *
 *  NOTA: Non ho trovato un modo migliore al momento per dargli uno scriptable object di default, se non eseguire la
 *  start nell'editor e fargli trovare l'asset di default.
 *
 * NOTA: Ho dovuto disabilitare nelle preferences della fisica ho "Query Hit Triggers".
 * Non funzionava infatti con lo zombie che ha un grosso collider di allarme trigger intorno,
 * e la OnMouseEnter veniva chiamata anche quando il mouse era lontano dallo zombie, ma dentro la
 * sfera del trigger.
 */
[ExecuteInEditMode]
public class HighlitedSelectable : MonoBehaviour
{
    [SerializeField]
    HighlightSettings settings;
    EventInstance clickSound;
    
    void Start()
    {
        #if UNITY_EDITOR
        if (settings == null)
        {
            // TODO:  Gestisci l'assenza delle settings
            settings = AssetDatabase.LoadAssetAtPath<HighlightSettings>("Assets/Config/HighlightSettings.asset");
        }
        #endif
        clickSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.yes);
    }
    
    void OnMouseEnter()
    {
        settings.material.color = settings.highlightedColor;
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Highlight"));
    }

    /*
    void OnMouseOver()
    {
        Debug.Log("Il Mouse è over!");
    }
    */
    
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
