using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class WalkInCustomer : Customer
    {
        public WalkInCustomer(string id, string name, string number) : base(id, name, number)
        {
            
        }

        public override bool CanAccruePoints()
        {
            return false;
        }
        public override decimal GetDiscountPercent()
        {
            return 0m;
        }
    }
}
