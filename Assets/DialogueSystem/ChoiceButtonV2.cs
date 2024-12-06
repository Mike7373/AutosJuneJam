using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChoiceButtonV2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Elements")] 
    [SerializeField] private TMP_Text _buttonText;

    [Header("Button Settings")] 
    [SerializeField] private Color _normalTextColor = Color.black;
    [SerializeField] private Color _hoverTextColor = Color.white;
    //Aggiungere unità di misura (px)
    [SerializeField] private int _normalTextSize = 40;
    [SerializeField] private int _hoverTextSize = 40;

    DialogueBrainV2 _dialogueBrain;
    private Button _button;
    private Choice _choice;

    private void Start()
    {
        _buttonText.color = _normalTextColor;
        _buttonText.fontSize = _normalTextSize;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
        _dialogueBrain = FindObjectOfType<DialogueBrainV2>();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Inizializza il bottone
    /// </summary>
    /// <param name="choice"></param>
    /// <param name="prefix"></param>
    public void Initialize(Choice choice, int prefix)
    {
        GetComponentInChildren<TMP_Text>().text = $"{prefix}. {choice.text}";
        _choice = choice;
    }
    
    /// <summary>
    /// Invoca AnswerEvent proprietario di DialogueBrain
    /// </summary>
    private void OnButtonClicked()
    {
        _dialogueBrain.AnswerEvent.Invoke(_choice.nextSentence);
    }

    #region Obsolete Methods

    /// <summary>
    /// Ridimensiona la dimensione di un bottone in base allo spazio e alla quantità di bottoni che devono essere generati.
    /// </summary>
    /// <param name="boxSize"></param>
    /// <param name="parts"></param>
    /// <param name="xPadding"></param>
    /// <param name="yPadding"></param>
    [Obsolete]
    public void ResizeButton(Vector2 boxSize, int parts, float xPadding, float yPadding)
    {
        float sizeX = (boxSize.x - xPadding) / parts;
        float sizeY = boxSize.y - yPadding;
        GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
    }

    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonText.color = _hoverTextColor;
        _buttonText.fontSize = _hoverTextSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonText.color = _normalTextColor;
        _buttonText.fontSize = _normalTextSize;
    }
}