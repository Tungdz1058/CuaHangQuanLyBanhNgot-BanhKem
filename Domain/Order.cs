using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Generic;
using QuanLyCuaHangBanhNgot_BanhKem.Pricing;
using QuanLyCuaHangBanhNgot_BanhKem.Service;
namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class Order : IEntity<string>
    {
        public event EventHandler<OrderChangeStatus> _OrderChangeStatus;
        public OrderService service { get; private set; }
        public string OrderID { get; private set; }
        public DateTime OrderDate { get; set; }
        public Customer customer { get; private set; }

        public decimal CustomerDiscount { get; private  set; }
        public OrderStatus status { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal OrderLevelDiscount { get; private set; }
        public decimal ShippingFee { get; private set; }
        public decimal VATAmount { get; private set; }

        public decimal Adress { get; set; }
        public bool IsShipping { get; private set; }
        

        
        public List<OrderLine> lines = new List<OrderLine>();

        private readonly IPriceRule price_rule;
        private readonly ITaxCaculators _taxCal;
        private readonly IShippingFee _shipping;
        private readonly IOrderDiscountPolicy _orderdiscount;

        public decimal Total { get; private set; }

        public string ID => OrderID;
        public Order(string OrderId,
                     OrderStatus status,
                     IPriceRule price_rule,
                     IShippingFee _shipping,
                     IOrderDiscountPolicy _orderdiscount,
                     ITaxCaculators _taxCal,
                     bool IsShipping)
        {
            this.status = status;
            this.OrderID = OrderId;
            this.price_rule = price_rule;
            this._shipping = _shipping;
            this._orderdiscount = _orderdiscount;
            this.IsShipping = IsShipping;
        }

        public void AddLine(OrderLine line)
        {
            if (status != OrderStatus.Draft)
                throw new InvalidOperationException("Cannot modify lines when not in Draft");

            lines.Add(line);
        }

        public void ChangeStatus(OrderStatus newstatus)
        {
            if (status == newstatus) return;
            else
            {
                var old = status;
                var handler = _OrderChangeStatus;
                handler?.Invoke(this, new OrderChangeStatus(this,old,newstatus));
            }
        }
        public decimal Recalculate()
        {
            foreach (var line in lines) { Subtotal = price_rule.LineAmount(line); }
            OrderLevelDiscount = _orderdiscount.orderdiscountpercent(this);
            VATAmount = _taxCal.VATCal()*Subtotal;
            if (IsShipping) ShippingFee = (Adress > 3m) ? (30m + _shipping.ShippingCompute(Adress - 3m)) : 30m;
            else ShippingFee = 0m;
            CustomerDiscount = customer.GetDiscountPercent() * Subtotal;
            decimal taxable = Subtotal - CustomerDiscount - OrderLevelDiscount + ShippingFee;
            Total = taxable + VATAmount;
            return Total;
        }
    }
}
