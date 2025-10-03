using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Domain;

namespace QuanLyCuaHangBanhNgot_BanhKem.Receipts
{
    public class DetailReceiptFormatter : IReceiptFormatter
    {
        public Order order { get; private set; }
        public Customer customer { get; private set; }
        public Payment payment { get; private set; }
        public DetailReceiptFormatter(Order order, Customer customer, Payment payment)
        {
            this.order = order;
            this.customer = customer;
            this.payment = payment;
        }
        public void PrintInfor()
        {
            Console.WriteLine("============== MEMBER ORDER RECEIPT ==============");
            Console.WriteLine($"Order ID               : {order.OrderID}");
            Console.WriteLine($"Member Name            : {customer.FullName}");
            Console.WriteLine($"Phone                  : {customer.Phone}");
            if (customer is MemberCustomer mcus)
            {
            Console.WriteLine($"Member Rank            : {mcus.tier}");
            Console.WriteLine($"Point Reward           : {mcus.Points - mcus.OldPoints}");
            Console.WriteLine($"Reward Points          : {mcus.Points}");
            }

            if (order.IsShipping)
            Console.WriteLine($"Shipping Address       : {order.Adress}");
            else
            Console.WriteLine("----         || Walk-in Member ||         ----");

            Console.WriteLine($"Order Date             : {order.OrderDate:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"List of Cakes          :");

            foreach (var line in order.lines)
            {
                Console.WriteLine($"    + {line.product.Name} | Size: {line.size} | Topping: {line.topping} | x{line.quantity}");
            }

            Console.WriteLine($"Member Discount        : {order.OrderLevelDiscount} VND");
            Console.WriteLine($"Shipping Fee           : {order.ShippingFee} VND");
            Console.WriteLine($"Total Payment          : {order.Total} VND");
            Console.WriteLine($"Order Status           : {order.status}");

            if (payment != null)
            {
            Console.WriteLine("Payment Info:");
            Console.WriteLine($"   Method              : {payment.method}");
            Console.WriteLine($"   Date                : {payment.PaidDateAt:dd/MM/yyyy HH:mm}");
            }
            else
            {
            Console.WriteLine($"Payment                : [Chưa thanh toán]");
            }

            Console.WriteLine("==================================================");
        }
    }
}
