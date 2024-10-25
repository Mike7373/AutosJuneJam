using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] private ChoiceButton _choiceButtonPrefab;
    [SerializeField] private GameObject _buttonsContainer;
    [SerializeField] private VerticalLayoutGroup _layoutGroup;
    private RectTransform _boxSize;
    public static List<ChoiceButton> currentButtons = new();

    private void Start()
    {
        _boxSize = _buttonsContainer.GetComponent<RectTransform>();
    }

    public void SpawnButtons(List<Choice> buttons)
    {
        float buttonHeight = 0;
        for (int i = 0; i < buttons.Count; i++)
        {
            ChoiceButton button = Instantiate(_choiceButtonPrefab, _buttonsContainer.transform);
            button.Initialize(buttons[i], i+1);
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

    public static void ClearButtons()
    {
        foreach (var button in currentButtons)
        {
            Destroy(button.gameObject);
        }
        currentButtons.Clear();
    }
}