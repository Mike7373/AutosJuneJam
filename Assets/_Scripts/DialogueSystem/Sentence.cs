using System;
using UnityEngine;

[Serializable]
public class Sentence
{
    public DialogueActor actor;
    [TextArea(5,10)]
    public string text;
}
