using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * 
 *
 *
 *  Quando si istanzia un dialogo bisogna mappare le componenti degli attori in gioco con gli id degli attori nel dialogo.
 *  Il mapping può avvernire anche al momento, non serve un preprocessing del dialogo.
 *
 *  Questo con l'obbiettivo di poter riusare dialoghi generici e rimappare dialoghi su attori diversi:
 *
 *  Uno script dice al dialogue system:
 *      Fammi iniziare il dialogo X, e all'actorId "xXX" corrisponde questo attore qui con questa icona e nome. (Cioè Il mapping.)
 *      Nel mapping indica anche chi è il player.
 *  
 *
 *  Per parlare serve:
 *
 *    Una componente di "Attore" che dice nome e icona. Contiene le informazioni del mapping.
 *
 *    Una componente di "Dialogo" che indica l'asset con il dialogo.
 *  Se volessimo cliccare su un gameobject e iniziare un dialogo potremmo avere la componente "Dialogo". 
 *      - Quando gli viene assegnato un TextAsset da inspector, lo parsa e sempre da inspector apre la finestra
 *          del mapping. Per ogni actorId presente nel dialogo viene richiesta la relativa componente "Attore",
 *          sempre da inspector popoliamo i campi.
 *
 *      - Se l'oggetto assegnato ha la componente "Attore", effettua un'euristica per il mapping.
 *
 *      Caso d'uso 1: Parlare con una sola persona.
 *          - Assegno componente "Attore"
 *          - Assegno componente "Dialogo"
 *          - Assegno Text Asset.
 *          - Popolo il mapping nella componente "Dialogo"
 *          - Opzionalmente, può partire un'euristica per popolare automaticamente il mapping, es:
 *              Negli asset dei dialoghi gli actor id adottano una convenzione: actorId: "player", "other" ecc..
 *      
 * 
 *      Caso d'uso 2: Parlare con un gruppo di persone.
 *      Nel caso del gruppo di persone che parlano in scena, con Maw J Laygo faccio così:
 *          - Asegno la componente "Dialogo" all'oggetto che raggruppa le persone che parlano.
 *          - Assegno anche l'highlight selectable, che fa quindi l'highlight di tutti i figli cioè le persone che parlano.
 *          - Assegno l'asset del dialogo alla componente "Dialogo", che mostrerà un'ulteriore proprietà, il mapping.
 *          - Assegno la componente attore a ciascun membro del gruppo.
 *          - Trascino nell'inspector ciascun attore nel mapping della componente Dialogo.
 *     
 *  
 * 
 */


public class Parlable : MonoBehaviour
{
    [SerializeField]
    TextAsset dialogueJson;
    
    //[SerializeField]
    //Actor
    
    Dictionary<string, Sentence> _currentDialogue = new();

    public void StartDialogue()
    {
        Dialogue dialogue = JsonUtility.FromJson<Dialogue>(dialogueJson.text);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
