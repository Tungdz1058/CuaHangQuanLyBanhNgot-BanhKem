using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Pricing
{
    public class InnerCityShipping : IShippingFee
    {
        public decimal ShippingCompute(decimal distance)
        {
            return distance * 10m;
        }
    }
}
