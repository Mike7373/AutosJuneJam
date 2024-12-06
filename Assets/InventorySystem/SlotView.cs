using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace InventorySystem
{
    public class SlotView : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private GameObject _data;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _amount;

        public Image Icon => _icon;
        
        /// <summary>
        /// Set the content with default values
        /// </summary>
        public void ClearContent()
        {
            _icon.sprite = null;
            _amount.text = "0";
        }
        
        /// <summary>
        /// Set the visibility of _data GameObject 
        /// </summary>
        /// <param name="toggle"></param>
        public void ToggleContent(bool toggle)
        {
            _data.SetActive(toggle);
        }
    }
}