using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Pricing;
namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class Order
    {
        public string OrderNo { get; private set; }
        public DateTime OrderDate { get; private set; }
        public Customer customer { get; private set; }

        public decimal CustomerDiscount { get; private  set; }
        public OrderStatus status { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal OrderLevelDiscount { get; private set; }
        public decimal ShippingFee { get; private set; }
        public decimal VATAmount { get; private set; }

        public decimal Adress { get; private set; }

        
        public List<OrderLine> lines = new List<OrderLine>();

        private readonly IPriceRule price_rule;
        private readonly ITaxCaculators _taxCal;
        private readonly IShippingFee _shipping;
        private readonly IOrderDiscountPolicy _orderdiscount;

        public decimal Total { get; private set; }


        public decimal Recalculate()
        {
            foreach (var line in lines) { decimal Subtotal = price_rule.LineAmount(line); }
            OrderLevelDiscount = _orderdiscount.orderdiscountpercent(this);
            VATAmount = _taxCal.VATCal()*Subtotal;
            ShippingFee = (Adress > 3m) ? (30m + _shipping.ShippingCompute(Adress - 3m)) : 30m;
            CustomerDiscount = customer.GetDiscountPercent() * Subtotal;
            decimal taxable = Subtotal - CustomerDiscount - OrderLevelDiscount + ShippingFee;
            Total = taxable + VATAmount;
            return Total;
        }
    }
}
