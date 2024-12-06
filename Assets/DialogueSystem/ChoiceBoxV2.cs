using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBoxV2 : MonoBehaviour
{
    [SerializeField] private ChoiceButtonV2 _choiceButtonPrefab;
    [SerializeField] private RectTransform _buttonsContainer;
    [SerializeField] private VerticalLayoutGroup _layoutGroup;
    
    /// <summary>
    /// Istanzia i prefab delle choice in quantità pari al numero di choice previste nel dialogo
    /// </summary>
    /// <param name="choices"></param>
    public void SpawnButtons(List<Choice> choices)
    {
        float buttonHeight = 0;
        for (int i = 0; i < choices.Count; i++)
        {
            ChoiceButtonV2 button = Instantiate(_choiceButtonPrefab, _buttonsContainer.transform);
            button.Initialize(choices[i], i+1);
            
            if (i == 0)
            {
                button.GetComponent<Button>().Select();
                buttonHeight = button.GetComponent<RectTransform>().sizeDelta.y;
            }
        }
        ResizeHeight(choices.Count, buttonHeight);
    }

    /// <summary>
    /// Ridimensiona l'altezza della box delle risposte in modo che possno essere visualizzate sempre tutte le opzioni in modo corretto
    /// TODO: Deve farlo la UI con il layout
    /// </summary>
    /// <param name="children"></param>
    /// <param name="childHeight"></param>
    private void ResizeHeight(int children, float childHeight)
    {
        _buttonsContainer.sizeDelta = new Vector2(_buttonsContainer.sizeDelta.x, children * childHeight + _layoutGroup.spacing*children);
    }

    /// <summary>
    /// Elimina tutte le istanze delle risposte correnti.
    /// </summary>
    public void ClearButtons()
    {
        foreach (GameObject button in _buttonsContainer.transform)
        {
            Destroy(button);
        }
    }
}