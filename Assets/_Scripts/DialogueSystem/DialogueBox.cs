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
    //[SerializeField] private float _actorIconResizeValue = 0.4f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void ShowArrow()
    {
        _arrow.SetActive(true);
    }
    
    public void HideArrow()
    {
        _arrow.SetActive(false);
    }
}