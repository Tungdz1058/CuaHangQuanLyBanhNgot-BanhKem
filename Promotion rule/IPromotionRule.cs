using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Promotion_rule
{
    public interface IPromotionRule<OrderContext>
    {
        void ApplyPromotionRule(OrderContext context);
    }

}
