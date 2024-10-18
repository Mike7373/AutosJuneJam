using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChoiceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Elements")] 
    [SerializeField] private TMP_Text _buttonText;

    [Header("Button Settings")] 
    [SerializeField] private Color _normalTextColor = Color.black;
    [SerializeField] private Color _hoverTextColor = Color.white;
    //Aggiungere unità di misura (px)
    [SerializeField] private int _normalTextSize = 40;
    [SerializeField] private int _hoverTextSize = 40;

    private static List<GameObject> _currentButtons = new();
    private Button _button;

    private void Start()
    {
        _buttonText.color = _normalTextColor;
        _buttonText.fontSize = _normalTextSize;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _currentButtons.Add(gameObject);
    }

    public void OnClick()
    {
        DialogueBrain.AnswerEvent.Invoke(DialogueBrain.GetAnswer(_button.GetComponentInChildren<TMP_Text>().text));
        foreach (GameObject go in _currentButtons)
        {
            Destroy(go);
        }

        _currentButtons.Clear();
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