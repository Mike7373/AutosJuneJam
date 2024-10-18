using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChoiceButton : MonoBehaviour
{
    private static List<GameObject> _currentButtons = new();
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _currentButtons.Add(gameObject);
    }

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
        GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX,sizeY);
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
}