using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class InventoryLowEventArgs : EventArgs
    {
        public CakeProduct product{ get; private set; }
            public InventoryLowEventArgs(CakeProduct product)
            {
            this.product = product;
            } 

        }
}
