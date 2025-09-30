using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Domain;

namespace QuanLyCuaHangBanhNgot_BanhKem.Generic
{
    public class InMemoryInventory<T, Tkey> : IRepository<T, Tkey> where T : IEntity<Tkey>
    {
        private List<T> inventory = new List<T>();
        
        public void Add(T item)
        {
            inventory.Add(item);
        }

        public void Remove(Tkey id) 
        {
            T item = GetById(id);
            if(item!=null) inventory.Remove(item);
        }


        public IEnumerable<T> GetAll()
        {
            return inventory;
        }

        public T GetById(Tkey id)
        {
            return inventory.FirstOrDefault(x => x.ID.Equals(id));
        }

        public void Update(Tkey id,T newproduct)
        {
            var index =inventory.FindIndex(x => x.ID.Equals(id));
            if (index != -1) inventory[index] = newproduct;
        }
    }
}
