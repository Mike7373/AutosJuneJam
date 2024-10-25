using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] private GameObject _choiceButtonPrefab;
    [SerializeField] private GameObject _buttonsContainer;
    [SerializeField] private VerticalLayoutGroup _layoutGroup;
    private RectTransform _boxSize;

    private void Start()
    {
        _boxSize = _buttonsContainer.GetComponent<RectTransform>();
    }

    public void SpawnButtons(List<Choice> buttons)
    {
        float buttonHeight = 0;
        for (int i = 0; i < buttons.Count; i++)
        {
            GameObject button = Instantiate(_choiceButtonPrefab, _buttonsContainer.transform);
            button.GetComponentInChildren<TMP_Text>().text = $"{i+1}. {buttons[i].text}";
            if (i == 0)
            {
                button.GetComponent<Button>().Select();
                buttonHeight = button.GetComponent<RectTransform>().sizeDelta.y;
            }
        }
        ResizeHeight(buttons.Count, buttonHeight);
    }

    public void ResizeHeight(int children, float childHeight)
    {
        _boxSize.sizeDelta = new Vector2(_boxSize.sizeDelta.x, children * childHeight + _layoutGroup.spacing*children);
    }
}