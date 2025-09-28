using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Repository
{
    public interface IEntity<Tkey>
    {
        Tkey ID { get; }
    }
}
