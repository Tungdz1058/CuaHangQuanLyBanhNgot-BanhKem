using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using QuanLyCuaHangBanhNgot_BanhKem.Generic;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class Payment : IEntity<string>
    {
        public string paymentid { get; private set; }
        public Order order { get; private set; }
        public PaymentMethod method { get; private set; }

        public decimal Amount { get; private set; }
        public DateTime PaidDateAt { get; private set; }

        public string ID => paymentid;

    
        public Payment(PaymentMethod method, DateTime time, string paymentid, Order order)
        {
            this.method = method;
            this.order = order;
            PaidDateAt = time;
            Amount = order.Total;
            this.paymentid = paymentid;
        }
    }
}
