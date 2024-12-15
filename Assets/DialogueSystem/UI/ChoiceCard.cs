using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceCard : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] TMP_Text choiceTitle;
    
    public void Initialize(List<Choice> currentSentenceChoices, DialogueEngine.DialogueState dialogueState)
    {
        throw new System.NotImplementedException();
    }
    
}
