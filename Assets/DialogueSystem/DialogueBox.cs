using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Nella nuova dialogue box 
 */
public class DialogueBox : MonoBehaviour
{
    public Image leftPortrait;
    public Image rightPortrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    
    public ChoiceBox choiceBox;
    [SerializeField] private GameObject _arrow;

    private void Awake()
    {
        var other = FindObjectOfType<DialogueBox>();
        if (other != this)
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