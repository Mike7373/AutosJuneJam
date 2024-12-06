using UnityEngine;

namespace InventorySystem
{
    //Leaf
    public class Item : InventoryComponent
    {
        public Sprite Icon { get; }
        public int Amount { get; }
        
        
        public Item(string name, Sprite icon)
        {
            ID = $"ITM{_instancesCount++}";
            Name = name;
            Icon = icon;
            Amount = 0;
        }
        
        public override void Add(InventoryComponent component)
        {
            Debug.LogError($"You can't use the method Add() on a leaf!");
        }

        public override void Remove(InventoryComponent component)
        {
            Debug.LogError($"You can't use the method Add() on a leaf!");
        }
    }
}
