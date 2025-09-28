using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Pricing
{
    public interface IShippingFee
    {
        decimal ShippingCompute(decimal distance);
    }
}
