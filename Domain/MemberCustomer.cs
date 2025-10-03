using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class MemberCustomer : Customer
    {
        public string MemberCode { get; private set; }
        public MemberTier tier { get; private set; }
        public decimal OldPoints { get; private set; }
        public decimal Points { get; private set; }
        public decimal CreditLimit = 500m;

        public MemberCustomer(string id, string name, string number, string code, int point, MemberTier tier) : base(id, name, number)
        {
            this.MemberCode = code;
            this.Points = point;
            this.tier = tier;
        }

        public override bool CanAccruePoints()
        {
            return true;
        }
        public override void AddPoints(decimal pts) 
        {
            OldPoints = Points;
            this.Points += Math.Round(pts);
        }
        public override decimal GetDiscountPercent()
        {
            switch (tier)
            {
                case MemberTier.Standard:
                    return 0.02m;
                case MemberTier.Silver:
                    return 0.04m;
                case MemberTier.Gold:
                    return 0.06m;
                default:
                    return 0m;

            }
        }
    }
}
