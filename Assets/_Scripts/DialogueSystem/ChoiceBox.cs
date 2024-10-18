using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] private GameObject _choiceButtonPrefab;
    [SerializeField] private GameObject _buttonsContainer;
    [SerializeField] private GridLayoutGroup _layout;
    [SerializeField] private int _buttonDistance;
    private RectTransform _boxSize;

    private void Start()
    {
        _boxSize = GetComponent<RectTransform>();
    }

    public void SpawnButtons(List<Choice> buttons)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            GameObject button = Instantiate(_choiceButtonPrefab, _buttonsContainer.transform);
            button.GetComponentInChildren<TMP_Text>().text = buttons[i].text;
            if (i == 0)
            {
                button.GetComponent<Button>().Select();
            }
        }
    }
}