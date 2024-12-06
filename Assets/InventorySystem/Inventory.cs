namespace InventorySystem
{
    public class Inventory : InventoryCompontent
    {
        public Inventory(string name)
        {
            ID = $"INV{_instancesCount++}";
            Name = name;
        }
    }
}
