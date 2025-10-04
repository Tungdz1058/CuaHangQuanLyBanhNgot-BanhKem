using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Domain;

namespace QuanLyCuaHangBanhNgot_BanhKem.Pricing
{
    public class DefaultPriceRule : IPriceRule
    {
        public decimal LineAmount(OrderLine line )
        {
            line.ComputeLineAmount();
            return line.LineAmount;
        }

    }
}
