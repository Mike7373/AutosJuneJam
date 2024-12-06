using UnityEngine;

namespace InventorySystem
{
    //Leaf
    public class Item : InventoryCompontent
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
        
        public override void Add(InventoryCompontent component)
        {
            Debug.LogError($"You can't use the method Add() on a leaf!");
        }

        public override void Remove(InventoryCompontent component)
        {
            Debug.LogError($"You can't use the method Add() on a leaf!");
        }
    }
}
