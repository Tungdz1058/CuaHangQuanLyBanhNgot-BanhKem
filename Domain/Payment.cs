using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class Payment
    {
        public string paymentid { get; private set; }
        public Order order { get; private set; }
        public PaymentMethod method { get; private set; }

        public decimal Amount { get; private set; }
        public DateTime PaidDateAt { get; private set; }
        private string payment()
        {
            switch (method)
            {
                case PaymentMethod.Cash:
                    return "CSH-001";
                case PaymentMethod.BankTransfer:
                    return "BTF-002";
                case PaymentMethod.EWallet:
                    return "EWL-003";
                default:
                    return "";
            }
        }
        public Payment(PaymentMethod method, DateTime time)
        {
            this.method = method;
            this.PaidDateAt = time;
            this.Amount = order.Total;
            this.paymentid = payment();
        }
    }
}
