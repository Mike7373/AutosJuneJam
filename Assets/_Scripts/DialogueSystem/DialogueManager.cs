using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    
    public DialogueCanvas DialogueCanvas;
    private List<Sentence> _currentDialogue;
    private Image _icon;
    private TMP_Text _speaker;
    private TMP_Text _text;
    private bool skip;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetupCanvas();
    }

    public void SetupCanvas()
    {
        _icon = DialogueCanvas.GetComponentInChildren<ActorIcon>().gameObject.GetComponent<Image>();
        _speaker = DialogueCanvas.GetComponentInChildren<ActorName>().gameObject.GetComponent<TMP_Text>();
        _text = DialogueCanvas.GetComponentInChildren<DialogueText>().gameObject.GetComponent<TMP_Text>();
    }

    public void SetDialogue(List<Sentence> dialogue)
    {
        _currentDialogue = dialogue;
    }

    public IEnumerator StartDialogue()
    {
        DialogueCanvas.gameObject.SetActive(true);
        foreach (Sentence sentence in _currentDialogue)
        {
            _icon.sprite = sentence.actor.GetIcon();
            _speaker.text = sentence.actor.GetActorName();
            _text.text = sentence.text;
            //attendo la pressione di un tasto che mette skip a true
            while (!skip)
            {
                yield return null;
            }
            skip = false;
        }
        DialogueCanvas.gameObject.SetActive(true);
    }
    
}
