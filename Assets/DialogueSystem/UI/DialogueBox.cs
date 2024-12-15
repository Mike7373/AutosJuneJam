using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] ScrollRect scrollView;
    [SerializeField] TMP_Text   dialogueTitle;

    [SerializeField] SentenceCard prefabChoiceDoneCard;
    [SerializeField] SentenceCard prefabSentenceCard;
    [SerializeField] ChoiceCard   prefabChoiceCard;

    DialogueEngine dialogueEngine;
    
    public void StartDialogue(Dialogue d, Dictionary<string, Actor> actors)
    {
        dialogueEngine = new DialogueEngine(d, actors);
        gameObject.SetActive(true);
        Initialize(dialogueEngine.dialogueState);
    }

    public void Initialize(DialogueEngine.DialogueState dState)
    {
        foreach (var item in dState.history)
        {
            InstantiateDialogueItem(item, dState);
        }
        // Creo la sentence corrente con le choice
        if (dState.currentSentence != null)
        {
            SentenceCard sc = Instantiate(prefabSentenceCard, scrollView.content);
            sc.Initialize(dState.currentSentence, dState);
            ChoiceCard cc = Instantiate(prefabChoiceCard, scrollView.content);
            cc.Initialize(dState.currentSentence.choices, dState);
        }
        else
        {
            gameObject.SetActive(false);
        }
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
                SentenceCard sc = Instantiate(prefabChoiceDoneCard, scrollView.content);
                sc.Initialize(choiceDone, dState);
                break;
            }
        }
    }
    
    // TODO: Rifare l'evento di answer che va nell'engine, in questo modo il resto della UI
    //       non dipende da questa specifica dialoguebox ma solo dall'engine
    public void Answer(Choice c)
    {
        DialogueEngine.DialogueState ds = dialogueEngine.Answer(c);
        
        // Distruggo nella UI la card con le scelte
        Destroy(scrollView.content.GetChild(scrollView.content.childCount - 1).gameObject);
        var dState = dialogueEngine.dialogueState;
        // Disegno la prossima frase con scelte
        if (dState.currentSentence != null)
        {
            SentenceCard sc = Instantiate(prefabSentenceCard, scrollView.content);
            sc.Initialize(dState.currentSentence, dState);
            ChoiceCard cc = Instantiate(prefabChoiceCard, scrollView.content);
            cc.Initialize(dState.currentSentence.choices, dState);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    

    // TODO: Freccina a fianco della scelta corrente, farla comparire a sinistra del numero?
    //[SerializeField] private GameObject _arrow;
}