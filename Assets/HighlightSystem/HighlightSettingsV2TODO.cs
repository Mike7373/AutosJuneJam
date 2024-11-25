using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

/**
 * Il sistema di highlight ottiene la sovrailluminazione di un oggetto, spostandolo nel layer "Highlight",
 * mentre una RenderFeature aggiuntiva si preoccupa di ridisegnare l'oggetto con il materiale di override.
 *
 * 
 */

//[CreateAssetMenu(fileName = "HighlightSettingsV2", menuName = "ScriptableObjects/HighlightSettingsV2")]
public class HighlightSettingsV2 : ScriptableObject
{
    public readonly string LayerName = "Highlight";
    public readonly string RenderObjectsName = "HighlightSystem";
    
    public UniversalRendererData urpRenderer;

    

    UniversalRendererData urpData;

    void CreateRenderObjects()
    {
        
        int idx = urpData.rendererFeatures.FindIndex(urf => urf is RenderObjects && urf.name == RenderObjectsName );
        if (idx == -1)
        {
            Debug.Log($"Creating RenderFeature {RenderObjectsName}...");
            //RenderObjects robs = 
        }
    }

}
    