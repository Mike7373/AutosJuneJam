using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] private ChoiceButton _choiceButtonPrefab;
    [SerializeField] private RectTransform _buttonsContainer;
    
    /// <summary>
    /// Istanzia i prefab delle choice in quantità pari al numero di choice previste nel dialogo
    /// </summary>
    /// <param name="choices"></param>
    public void SpawnButtons(List<Choice> choices)
    {
        for (int i = 0; i < choices.Count; i++)
        {
            ChoiceButton button = Instantiate(_choiceButtonPrefab, _buttonsContainer.transform);
            button.Initialize(choices[i], i+1);
            if (i == 0)
            {
                button.GetComponent<Button>().Select();
            }
        }
    }

    /// <summary>
    /// Elimina tutte le istanze delle risposte correnti.
    /// </summary>
    public void ClearButtons()
    {
        foreach (Transform button in _buttonsContainer.transform)
        {
            Destroy(button.gameObject);
        }
    }
}