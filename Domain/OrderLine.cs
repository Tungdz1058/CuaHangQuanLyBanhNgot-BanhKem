using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Pricing;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class OrderLine
    {
        public int LineNo { get; private set; }
        public CakeProduct product { get; private set; }
        public CakeSize size { get; private set; }
        public Topping topping { get; private set; }
        public int quantity
        {
            get { return quantity; }
            private set
            {
                if(quantity < 0)
                {
                    value = 1;
                    Console.WriteLine("Số lượng bánh phải lớn hơn 0!!!");
                }
                quantity = value;
            }
        }
        public decimal unit_price { get; private set; }
        public decimal line_discount_percent
        {
            get { return line_discount_percent; }
            private set
            {
                if(line_discount_percent < 0m||line_discount_percent > 0.5m)
                {
                    value = 0;
                    Console.WriteLine("số tiền giảm giá chỉ được trong khoảng [0 - 50%]");
                }
                line_discount_percent = value;
            }
        }
        public decimal LineAmount { get; private set; }

        

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
            LineAmount = quantity * (GetSizePrice()+((product.IsTopping) ? GetToppingPrice() : 0m)) * (1 - line_discount_percent); 
        }

    }
}
