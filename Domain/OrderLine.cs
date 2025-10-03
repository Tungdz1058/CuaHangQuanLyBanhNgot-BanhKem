using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Generic;
using QuanLyCuaHangBanhNgot_BanhKem.Pricing;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class OrderLine : IEntity<string>
    {
        public string LineId { get; private set; }
        public CakeProduct product { get; private set; }
        public CakeSize size { get; private set; }
        public Topping topping { get; private set; }
        public bool IsReward { get; private set; }
        public OrderLine(string Lineid, CakeProduct product, CakeSize size, Topping topping, int quantity, bool IsReward)
        {
            this.LineId = Lineid;
            this.product = product;
            this.size = size;
            this.topping = topping;
            this.Quantity = quantity;
            this.IsReward = IsReward;
            this.unit_price = product.UnitPrice;
        }
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            private set
            {
                if (value <= 0)
                {
                    _quantity = 1;
                    Console.WriteLine("Số lượng bánh phải lớn hơn 0!!! (auto set = 1)");
                }
                else
                {
                    _quantity = value;
                }
            }
        }
        public decimal unit_price;
        private decimal _lineDiscountPercent = 0.1m; 
        public decimal LineDiscountPercent
        {
            get => _lineDiscountPercent;
            set
            {
                if (!IsReward && (value < 0m || value > 0.5m))
                {
                    _lineDiscountPercent = 0m;
                    Console.WriteLine("❌ Số tiền giảm giá chỉ được trong khoảng [0 - 50%]");
                }
                else
                {
                    _lineDiscountPercent = value;
                }
            }
        }
        public decimal LineAmount { get; private set; }

        public string ID => LineId;

        private decimal GetToppingPrice()
        {
            switch (topping)
            {
                case Topping.cheese:
                    return 30m;
                case Topping.socola:
                    return 15m;
                case Topping.corn:
                    return 10m;
                case Topping.strawberry:
                    return 25m;
                default:
                    return 0m;
            }
        }

        private decimal GetSizePrice()
        {
            switch (size)   
            {
                case CakeSize.S:
                    return unit_price;
                case CakeSize.M:
                    return unit_price * 1.5m;
                case CakeSize.L:
                    return unit_price * 2m;
                default:
                    return unit_price;
            }
        }

        public void ComputeLineAmount()
        {
            LineAmount = Quantity * (GetSizePrice()+((product.IsTopping) ? GetToppingPrice() : 0m)) * (1 - LineDiscountPercent); 
        }

    }
}
