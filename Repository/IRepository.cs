using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Repository
{
    public interface IRepository<T,Tkey>
    {
        void Add(T item);
        void Remove(Tkey id);
        void Update(Tkey id, T product);
        T GetById(Tkey id);
        IEnumerable<T> GetAll();
    }
}
