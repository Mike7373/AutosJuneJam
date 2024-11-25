using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DialogueBrain : MonoBehaviour
{
    public DialogueBox dialogueBox;

    [Header("Dialogue List")] 
    [SerializeField]
    private List<TextAsset> _dialogues = new();
    private static Dictionary<string, Sentence> _currentDialogue = new();
    private static int _dialogueIndex;
    private static Sentence _currentSentence;
    public static readonly UnityEvent<string> AnswerEvent = new();

    private void OnEnable()
    {
        AnswerEvent.AddListener(SendAswer);
    }

    private void Start()
    {
        StartDialogue();
    }

    private void OnDisable()
    {
        AnswerEvent.RemoveListener(SendAswer);
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && _currentSentence.choices.Count <= 1)
        {
            AnswerEvent.Invoke(_currentSentence.choices[0].nextSentence);
        }
    }

    /// <summary>
    /// Avvia il dialogo da questa istanza
    /// </summary>
    /// <returns></returns>
    public void StartDialogue()
    {
        List<Sentence> dialogue = LoadCurrentDialogue();
        _currentDialogue = dialogue.ToDictionary(sentence => sentence.sentenceID);
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.ShowArrow();
        //REGOLA: la prima frase di ogni dialogo deve avere "0" come sentenceID
        _currentSentence = _currentDialogue["0"];
        dialogueBox.playerIcon.sprite = DialogueActor.PlayerActor.Icon;
        DialogueSetup();
    }

    
    public void StartDialogue(Dialogue d)
    {
        _currentDialogue = d.sentenceList.ToDictionary(sentence => sentence.sentenceID);
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.ShowArrow();
        //REGOLA: la prima frase di ogni dialogo deve avere "0" come sentenceID
        //FIX: Usare la lista
        _currentSentence = d.sentenceList[0];
        dialogueBox.playerIcon.sprite = DialogueActor.PlayerActor.Icon;
        DialogueSetup();
    }

    /// <summary>
    /// Chiude la dialogue box
    /// </summary>
    /// <param name="incrementIndex"> true -> _dialogueIndex++</param>
    public void EndDialogue(bool incrementIndex)
    {
        if (incrementIndex)
        {
            _dialogueIndex++;
        }
        dialogueBox.gameObject.SetActive(false);
    }

    /// <summary>
    /// Carica un dialogo dal file json assegnato a _dialogues
    /// </summary>
    /// <returns></returns>
    private List<Sentence> LoadCurrentDialogue()
    {
        if (_dialogues.Count > 0)
        {
            List<Sentence> dialogue = JsonUtility.FromJson<Dialogue>(_dialogues[_dialogueIndex].text).sentenceList;
            return dialogue;
        }

        Debug.LogError("You must assign at least 1 TextAsset to load it!");
        return null;
    }

    /// <summary>
    /// Carica la frase successiva con le relative risposte. Se non c'è una frase successiva chiude il dialogo.
    /// </summary>
    /// <param name="nextSentence"></param>
    public void SendAswer(string nextSentence)
    {
        if (nextSentence != "")
        {
            _currentSentence = _currentDialogue[nextSentence];
            ChoiceBox.ClearButtons();
            DialogueSetup();
        }
        else
        {
            EndDialogue(true);
        }
    }

    /// <summary>
    /// Carica dinamicamente i dati del dialogo dentro alla UI
    /// </summary>
    /// <returns></returns>
    private void DialogueSetup()
    {
        ChoiceBox choicebox = dialogueBox._choiceBox.GetComponent<ChoiceBox>();
        choicebox.gameObject.SetActive(false);
        dialogueBox.characterIcon.sprite = DialogueActor.FindActorByID(_currentSentence.actorID).Icon;
        dialogueBox.actorName.text = DialogueActor.FindActorByID(_currentSentence.actorID).ActorName;
        dialogueBox.dialogueText.text = _currentSentence.text;
        //All'ultima battuta del dialogo nascondo la freccina
        if (_currentSentence.choices.Count > 1 || _currentSentence.choices[0].text != "" )
        {
            choicebox.gameObject.SetActive(true);
            choicebox.SpawnButtons(_currentSentence.choices);
        }
        else if (_currentSentence.choices[0].nextSentence == "")
        {
            dialogueBox.HideArrow();
        }
    }
}