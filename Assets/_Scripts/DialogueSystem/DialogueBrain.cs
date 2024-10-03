using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueActor))]
public class DialogueBrain : MonoBehaviour
{
    public DialogueBox dialogueBox;

    [Header("Dialogue List")]
    [SerializeField] private List<TextAsset> _dialogues;

    private DialogueActor _actor;
    private List<Sentence> _currentDialogue;
    private int _dialogueIndex;
    private bool skip;

    private void Start()
    {
        _actor = GetComponent<DialogueActor>();
    }

    public IEnumerator StartDialogue()
    {
        dialogueBox.gameObject.SetActive(true);
        foreach (Sentence sentence in _currentDialogue)
        {
            dialogueBox._actorIcon.sprite = sentence.actor.GetIcon();
            dialogueBox._actorName.text = sentence.actor.GetActorName();
            dialogueBox._dialogueText.text = sentence.text;
            //attendo la pressione di un tasto che mette skip a true
            while (!skip)
            {
                yield return null;
            }
            skip = false;
        }
        dialogueBox.gameObject.SetActive(true);
    }
    
}
