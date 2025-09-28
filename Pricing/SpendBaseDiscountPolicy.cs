using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Domain;
namespace QuanLyCuaHangBanhNgot_BanhKem.Pricing
{
    public class SpendBaseDiscountPolicy : IOrderDiscountPolicy
    {
        public decimal orderdiscountpercent(Order order)
        {
            if (order.Subtotal >= 300m) return 0.1m;
            else return 0m;
        }
    }
}
