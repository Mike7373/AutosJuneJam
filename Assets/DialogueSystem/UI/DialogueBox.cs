using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] Image      leftPortrait,rightPortrait;
    [SerializeField] ScrollRect scrollView;
    [SerializeField] TMP_Text   dialogueTitle;

    [SerializeField] SentenceCard prefabSentenceCard;
    [SerializeField] ChoiceCard   prefabChoiceCard;

    DialogueEngine dialogueEngine;
    
    // TODO: I ritratti dei personaggi possiamo metterli nel dialogo stesso, insieme alla frase che hanno detto
    //       Così togliamo anche la isPlayer dagli attori e possiamo fare dialoghi tra più personaggi visivamente rappresentati.
    public void StartDialogue(Dialogue d, Dictionary<string, Actor> actors)
    {
        dialogueEngine = new DialogueEngine(d, actors);
        gameObject.SetActive(true);
        Initialize(dialogueEngine.dialogueState);
    }
    
    public void EndDialogue()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(DialogueEngine.DialogueState dState)
    {
        foreach (var item in dState.history)
        {
            InstantiateDialogueItem(item, dState);
        }
        SentenceCard sc = Instantiate(prefabSentenceCard, scrollView.content);
        sc.Initialize(dState.currentSentence, dState);
        ChoiceCard cc = Instantiate(prefabChoiceCard, scrollView.content);
        cc.Initialize(dState.currentSentence.choices, dState);
    }

    void InstantiateDialogueItem(object item, DialogueEngine.DialogueState dState)
    {
        switch (item)
        {
            case Sentence sentence:
            {
                SentenceCard sc = Instantiate(prefabSentenceCard, scrollView.content);
                sc.Initialize(sentence, dState);
                break;
            }
            case ChoiceDone choiceDone:
            {
                SentenceCard sc = Instantiate(prefabSentenceCard, scrollView.content);
                sc.Initialize(choiceDone, dState);
                break;
            }
        }
    }
    
    public void Answer(Choice c)
    {
        DialogueEngine.DialogueState ds = dialogueEngine.Answer(c);
        
        // Distruggo nella UI la card con le scelte
        Destroy(scrollView.content.GetChild(scrollView.content.childCount - 1).gameObject);
        // Disegno la prossima frase con scelte
        SentenceCard sc = Instantiate(prefabSentenceCard, scrollView.content);
        sc.Initialize(dialogueEngine.dialogueState.currentSentence, dialogueEngine.dialogueState);
        ChoiceCard cc = Instantiate(prefabChoiceCard, scrollView.content);
        cc.Initialize(dialogueEngine.dialogueState.currentSentence.choices, dialogueEngine.dialogueState);
    }
    

    // TODO: Freccina a fianco della scelta corrente, farla comparire a sinistra del numero?
    //[SerializeField] private GameObject _arrow;
    
}