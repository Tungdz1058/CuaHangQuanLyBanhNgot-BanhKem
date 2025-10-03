using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Domain;
using QuanLyCuaHangBanhNgot_BanhKem.Service;

namespace QuanLyCuaHangBanhNgot_BanhKem.Promotion_rule
{
    public class Buy10Get1Free : IPromotionRule<Order>
    {
        public Order order { get; private set; }
        public Buy10Get1Free(Order order)
        {
            this.order = order;
        }
        public void ApplyPromotionRule(Order context)
        {
            if (context == null) throw new InvalidOperationException("Order context not found!!");
            int len = context.lines.Count;
            for(int i = 0; i< len;i++)
            {
                if (order.lines[i].IsReward)
                {
                    var newline = new OrderLine(order.lines[i].LineId, order.lines[i].product, CakeSize.L,Topping.cheese,1, order.lines[i].IsReward);
                    newline.LineDiscountPercent = 1m;
                    order.lines.Add(newline);
                    Console.WriteLine($"Reward x1 {order.lines[i].product.Name} - Size : {CakeSize.L} - Topping : {Topping.cheese}");
                }
            }
        }
    }
}
