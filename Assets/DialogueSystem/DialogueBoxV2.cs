using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxV2 : MonoBehaviour
{
    public Image playerIcon;
    public Image characterIcon;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    
    public ChoiceBoxV2 choiceBox;
    [SerializeField] private GameObject _arrow;

    private void Awake()
    {
        if (FindObjectOfType<DialogueBoxV2>() != this)
        {
            Debug.LogWarning("Esiste già un'altra dialogue box in scena, mi distruggo!");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Attiva la freccina nel dialogo
    /// </summary>
    public void ShowArrow()
    {
        _arrow.SetActive(true);
    }
    
    /// <summary>
    /// Disattiva la freccina nel dialogo
    /// </summary>
    public void HideArrow()
    {
        _arrow.SetActive(false);
    }
}