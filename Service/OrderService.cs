using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Generic;
using QuanLyCuaHangBanhNgot_BanhKem.Domain;
using QuanLyCuaHangBanhNgot_BanhKem.Pricing;
using QuanLyCuaHangBanhNgot_BanhKem.Transaction;
using QuanLyCuaHangBanhNgot_BanhKem.Receipts;
using QuanLyCuaHangBanhNgot_BanhKem.Promotion_rule;


namespace QuanLyCuaHangBanhNgot_BanhKem.Service
{
    public class OrderService
    {
        public InMemoryInventory<Order, string> _repoOrder = new InMemoryInventory<Order, string>(); 
        public InMemoryInventory<Customer, string> _repoCustomer = new InMemoryInventory<Customer, string>();
        public InMemoryInventory<CakeProduct, string> _repoProduct = new InMemoryInventory<CakeProduct, string>();
        public InMemoryInventory<Payment, string> _repoPayment = new InMemoryInventory<Payment, string>();
        InventoryService InService = new InventoryService();
        public event EventHandler<CanAccruePoint> AddPoint;
        public event EventHandler<OrderChangeStatus> _OrderChangeStatus;
        private readonly IPriceRule _pricerule;
        private readonly IOrderDiscountPolicy _OrderDiscount;
        private readonly ITaxCaculators _tax;
        private readonly IShippingFee _shippingfee;
        private readonly IPromotionRule<Order> _Promotion;

        private readonly IReceiptFormatter _format;

        public OrderService(IPriceRule pricerule,
                            IOrderDiscountPolicy orderDiscount,
                            ITaxCaculators tax,
                            IPromotionRule<Order> promotion,
                            IShippingFee shippingfee)
        {
            _pricerule = pricerule;
            _OrderDiscount = orderDiscount;
            _tax = tax;
            _shippingfee = shippingfee;
            _Promotion = promotion;
        }
        public void CreateOrder(string Customerid, bool IsShipping)
        {
            Console.WriteLine("Customers place orders!!");
            Customer customer = _repoCustomer.GetById(Customerid);
            if (customer == null) throw new InvalidOperationException("Customer not found!!");

            var NewOrder = new Order(Customerid,OrderStatus.Draft, _pricerule,_shippingfee,_OrderDiscount,_tax,IsShipping);
            NewOrder.OrderDate = DateTime.Now;
            if (NewOrder.IsShipping) NewOrder.Adress = int.Parse(Console.ReadLine());
            
            _repoOrder.Add(NewOrder);
        }
        public void AddLine(string Orderid,string ProductID,int quantity)
        {
            Order order = _repoOrder.GetById(Orderid);
            if (order == null) throw new InvalidOperationException("Order not found!!");
            CakeProduct product = _repoProduct.GetById(ProductID);
            if (product == null) throw new InvalidOperationException("Product not found!!");
            bool IsReward = (quantity >= 10) ? true : false;
            order.AddLine(new OrderLine(Orderid, product, product.size, product.topping,quantity,IsReward));
            _Promotion.ApplyPromotionRule(order);
            _repoOrder.Update(Orderid,order);
        }
        public void Confirmed(string OrderID)
        {
            Order order = _repoOrder.GetById(OrderID);
            if (order == null) throw new InvalidOperationException("Order not found!!");
            if (order.status != OrderStatus.Draft) throw new InvalidOperationException("Only Draft can be Confirmed!!");

            foreach(var line in order.lines)
            {
                InService.DecreaseStock(line.product.ProductId, line.quantity);
            }
            order.ChangeStatus(OrderStatus.Confirmed);
            
            _repoOrder.Update(OrderID,order);
        }
        public void Pay(string OrderID,PaymentMethod payment)
        {
            Order order = _repoOrder.GetById(OrderID);
            if (order == null) throw new InvalidOperationException("Order not found!!");
            Customer customer = _repoCustomer.GetById(OrderID);
            if (customer == null) throw new InvalidOperationException("Customer not found!!");
            Payment NewPay = new Payment(payment, DateTime.Now, OrderID);
            _repoPayment.Add(NewPay);
            order.ChangeStatus(OrderStatus.Paid);
            TransactionService trans = new TransactionService();
            if (order.customer is MemberCustomer member)
            {
                member.AddPoints(order.Total / 10m);
                var handler = AddPoint;
                handler?.Invoke(this, new CanAccruePoint(Math.Round(order.Total / 10m), member.Points));
            }
            trans.AddTransaction(new OrderTransaction(OrderID, new DetailReceiptFormatter(order, customer, NewPay)));
        }
        public void Cancelled(string OrderID)
        {
            Order order = _repoOrder.GetById(OrderID);
            if (order == null) throw new InvalidOperationException("Order not found!!");
            if (order.status != OrderStatus.Draft) throw new InvalidOperationException("Only Draft can be Cancelled!!");

            order.ChangeStatus(OrderStatus.Cancelled);
            _repoOrder.Remove(OrderID);
        }
    }
}
