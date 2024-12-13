using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChoiceButton : MonoBehaviour
{
    [SerializeField] TMP_Text index;
    [SerializeField] TMP_Text content;
    
    DialogueBox dialogueBox;
    Choice choice;

    void Start()
    {
        dialogueBox = GetComponentInParent<DialogueBox>(true);
    }

    public void Initialize(Choice c, int prefix)
    {
        choice = c;
        index.text = $"{prefix}";
        content.text = choice.text;
    }
    
    public void PublishChoice()
    {
        dialogueBox.Answer(choice);
    }
    
}