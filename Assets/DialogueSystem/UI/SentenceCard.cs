using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentenceCard : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] TMP_Text sentenceTitle;
    [SerializeField] TMP_Text sentenceText;

    public void Initialize(Sentence sentence, DialogueEngine.DialogueState state)
    {
        var actor = state.actorMapping[sentence.actorID];
        portrait.sprite    = actor.actorData.portrait;
        sentenceTitle.text = actor.actorData.actorName;
        sentenceText.text  = sentence.text;
    }

    public void Initialize(ChoiceDone choiceDone, DialogueEngine.DialogueState dState)
    {
        throw new System.NotImplementedException();
    }
}
