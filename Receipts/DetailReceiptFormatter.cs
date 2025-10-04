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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      🧾   ORDER RECEIPT   🧾                   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.ResetColor();

            Console.WriteLine($"🔖 Order ID           : {order.OrderID}");
            Console.WriteLine($"👤 Member Name        : {customer.FullName}");
            Console.WriteLine($"📞 Phone              : {customer.Phone}");

            if (customer is MemberCustomer mcus)
            {
                Console.WriteLine($"🏅 Member Rank        : {mcus.tier}");
                Console.WriteLine($"✨ Point Reward       : {mcus.Points - mcus.OldPoints}");
                Console.WriteLine($"💰 Reward Points      : {mcus.Points}");
                Console.WriteLine($"💳 Rank Discount      : {order.CustomerDiscount} VND");

            }

            if (order.IsShipping)
                Console.WriteLine($"🚚 Shipping Address   : {order.Adress}");
            else
                Console.WriteLine("🏬 Order Type         : Walk-in Member");

            Console.WriteLine($"📅 Order Date         : {order.OrderDate:dd/MM/yyyy HH:mm}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n📋 List of Cakes:");
            Console.ResetColor();
            foreach (var line in order.lines)
            {
                Console.WriteLine($"   ➡️ {line.product.Name} | Size: {line.size} | Topping: {line.topping} | x{line.Quantity}");
            }

            Console.WriteLine("\n💸 Order Discount     : " + $"{order.OrderLevelDiscount:N0} VND");
            Console.WriteLine($"🚚 Shipping Fee       : {order.ShippingFee:N0} VND");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"💵 Total Payment      : {order.Total:N0} VND");
            Console.ResetColor();
            Console.WriteLine($"📌 Order Status       : {order.status}");

            if (payment != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n💳 Payment Info:");
                Console.ResetColor();
                Console.WriteLine($"   Method             : {payment.method}");
                Console.WriteLine($"   Date               : {payment.PaidDateAt:dd/MM/yyyy HH:mm}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Payment            : [Chưa thanh toán]");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("════════════════════════════════════════════════════════════");
            Console.ResetColor();

        }
    }
}
