using System;
using System.Collections.Generic;

[Serializable]
public class Sentence
{
    public string actorID;
    public string sentenceID;
    public string text;
    public List<Choice> choices = new();
}