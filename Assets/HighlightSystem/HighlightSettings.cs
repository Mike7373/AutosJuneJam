using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "HighlightSettings", menuName = "ScriptableObjects/HighlightSettings", order = 1)]
public class HighlightSettings : ScriptableObject
{
    //      HighlightSystem.InitializeRenderPass crea la render feature dentro urp
    public Material material;

    public Color highlightedColor, selectedColor;

}
