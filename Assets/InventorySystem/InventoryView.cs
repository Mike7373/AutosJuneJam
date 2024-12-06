using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class InventoryView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private List<SlotView> _slots;

        private Dictionary<Image, Item> _slotAssignment;
        private Inventory _inventory;
        
        private void Awake()
        {
            _inventory = new Inventory("Player Inventory");
        }

        private void Start()
        {
            ClearAllSlots();
            ToggleAllSlots(false);
            SetupSlotAssignment();
        }
        
        public void AssignItem(Image img, Item item)
        {

        }

        public void RemoveItemAssignmen()
        {
            
        }

        public void ToggleAllSlots(bool toggle)
        {
            foreach (SlotView slot in _slots)
            {
                slot.ToggleContent(toggle);
            }
        }

        public void ClearAllSlots()
        {
            foreach (SlotView slot in _slots)
            {
                slot.ClearContent();
            }
        }

        public void SetupSlotAssignment()
        {
            _slotAssignment = _slots.ToDictionary(s =>)
            foreach (SlotView slot in _slots)
            {
                _slotAssignment.
                Debug.Log($"{_slotAssignment[slot.Icon]}");
            }
        }
    }
}
