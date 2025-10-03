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
            foreach (var line in order.lines)
            {
                if (line.IsReward)
                {
                    var newline = new OrderLine(line.LineId, line.product, CakeSize.L,Topping.cheese,1,line.IsReward);
                    newline.line_discount_percent = 1m;
                    order.lines.Add(newline);
                    Console.WriteLine($"Reward x1 {line.product.Name} - Size : {CakeSize.L} - Topping : {Topping.cheese}");
                }
            }
        }
    }
}
