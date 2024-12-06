using System.Collections.Generic;

namespace InventorySystem
{
    public abstract class InventoryCompontent
    {
        protected static int _instancesCount = 0;
        protected readonly List<InventoryCompontent> _invComponents;
        public string Name { get; protected set; }
        public string ID { get; protected set; }
        

        public List<InventoryCompontent> GetComponents()
        {
            return _invComponents;
        }

        public virtual void Add(InventoryCompontent component)
        {
            _invComponents.Add(component);
        }
        
        public virtual void Remove(InventoryCompontent component)
        {
            _invComponents.Remove(component);
        }
    }
}
