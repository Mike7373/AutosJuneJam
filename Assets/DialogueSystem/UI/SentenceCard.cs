using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class SentenceCard : MonoBehaviour
{
    public void Initialize(Sentence sentence, DialogueEngine.DialogueState state)
    {
    }

    public void Initialize(ChoiceDone choiceDone, DialogueEngine.DialogueState dState)
    {
        throw new System.NotImplementedException();
    }
}
