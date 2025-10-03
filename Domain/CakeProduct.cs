using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Generic;
namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class CakeProduct : IEntity<string>
    {
        public string ProductId { get;set; }
        public string Name { get; set; }
        public CakeType caketype { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQty { get; set; }
        public int ReorderThreshold { get; private set; } = 10;
        public bool IsActive { get; set; }
        public bool IsTopping { get; set; }
        public CakeSize size { get; private set; }
        public Topping topping { get; private set; }

        public string ID => ProductId;

        public virtual void DeductStock(int amount)
        {
            this.StockQty -= amount;
        }
        public virtual bool IsLowStock() => StockQty < ReorderThreshold;

    }
}
