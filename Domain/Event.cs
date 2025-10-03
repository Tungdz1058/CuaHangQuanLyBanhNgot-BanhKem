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

    public class CanAccruePoint : EventArgs
    {
        public decimal points { get; private set;}
        public decimal total { get; private set; }
        public CanAccruePoint(decimal points,decimal total)
        {
            this.points = points;
            this.total = total;
        }
    }
    public class OrderChangeStatus : EventArgs
    {
        public Order order { get; private set; }
        public OrderStatus old { get; private set; }
        public OrderStatus newstatus { get; private set; }
        public OrderChangeStatus(Order order, OrderStatus old, OrderStatus newstatus)
        {
            this.order = order;
            this.old = old;
            this.newstatus = newstatus;
        }
    }
}
