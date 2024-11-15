using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox Instance;
    public Image playerIcon;
    public Image characterIcon;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public GameObject _choiceBox;
    [SerializeField] private GameObject _arrow;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
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