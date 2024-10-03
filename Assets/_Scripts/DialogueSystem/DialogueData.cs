using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New dialogue")]
public class DialogueData : ScriptableObject
{
    
    public string dialogueName;
    public List<Sentence> dialogue;
}
