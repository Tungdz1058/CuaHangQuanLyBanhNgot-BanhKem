using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Pricing
{
    public class VATTax
    {
        public class TenVATPercent : ITaxCaculators 
        {
            public decimal VATCal()
            {
                return 0.1m;
            }
        }
        public class EightVATPercent : ITaxCaculators
        {
            public decimal VATCal()
            {
                return 0.08m;
            }
        }
    }
}
