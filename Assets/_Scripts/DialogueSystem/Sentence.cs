using System;
using UnityEngine;

[Serializable]
public class Sentence
{
    public string actorID;
    [TextArea(5,10)]
    public string text;
}
