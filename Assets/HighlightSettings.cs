using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighlightSettings", menuName = "ScriptableObjects/HighlightSettings", order = 1)]
public class HighlightSettings : ScriptableObject
{
    public Material material;

    public Color highlightedColor, selectedColor;

}
