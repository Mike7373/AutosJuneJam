namespace InventorySystem
{
    public class Inventory : InventoryComponent
    {
        public Inventory(string name)
        {
            ID = $"INV{_instancesCount++}";
            Name = name;
        }
    }
}
