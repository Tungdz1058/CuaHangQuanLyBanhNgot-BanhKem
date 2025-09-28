using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class CakeProduct
    {
        public string ProductId { get; private set; }
        public string Name { get; private set; }
        public CakeType caketype { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int StockQty { get; private set; }
        public int ReorderThreshold { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsTopping { get; private set; }
        public virtual void DeductStock(int qty) { }
        public virtual bool IsLowStock() => StockQty < ReorderThreshold;

    }
}
