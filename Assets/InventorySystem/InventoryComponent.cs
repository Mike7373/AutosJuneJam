using System.Collections.Generic;

namespace InventorySystem
{
    public abstract class InventoryComponent
    {
        protected static int _instancesCount = 0;
        protected readonly List<InventoryComponent> _invComponents;
        public string Name { get; protected set; }
        public string ID { get; protected set; }
        

        public List<InventoryComponent> GetComponents()
        {
            return _invComponents;
        }

        public virtual void Add(InventoryComponent component)
        {
            _invComponents.Add(component);
        }
        
        public virtual void Remove(InventoryComponent component)
        {
            _invComponents.Remove(component);
        }
    }
}
