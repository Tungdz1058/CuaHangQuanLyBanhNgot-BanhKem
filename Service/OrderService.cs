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
        private readonly IRepository<CakeProduct, string> _repoProduct;
        private readonly IRepository<Order, string> _repoOrder;
        private readonly IRepository<Customer, string> _repoCustomer;
        private readonly IRepository<Payment, string> _repoPayment;
        private readonly InventoryService InService;
        public event EventHandler<CanAccruePoint> AddPoint;
        private readonly IPriceRule _pricerule;
        private readonly IOrderDiscountPolicy _OrderDiscount;
        private readonly ITaxCaculators _tax;
        private readonly IShippingFee _shippingfee;
        public  IPromotionRule<Order> _Promotion;
        public TransactionService trans;
        private readonly IReceiptFormatter _format;

        public OrderService(InventoryService InService,
                            TransactionService trans,
                            IPriceRule pricerule,
                            IOrderDiscountPolicy orderDiscount,
                            ITaxCaculators tax,
                            IShippingFee shippingfee,
                            IRepository<CakeProduct,string> _repoProduct,
                            IRepository<Order, string> _repoOrder,
                            IRepository<Customer, string> _repoCustomer,
                            IRepository<Payment, string> _repoPayment)
        {
            this.InService = InService;
            this.trans = trans;
            _pricerule = pricerule;
            _OrderDiscount = orderDiscount;
            _tax = tax;
            _shippingfee = shippingfee;
            this._repoOrder = _repoOrder;
            this._repoCustomer = _repoCustomer;
            this._repoProduct = _repoProduct;
            this._repoPayment = _repoPayment;
        }
        public void CreateOrder(string Customerid, bool IsShipping)
        {
            Console.WriteLine("Customers place orders!!");
            Customer customer = _repoCustomer.GetById(Customerid);
            if (customer == null) throw new InvalidOperationException("Customer not found!!");
            var NewOrder = new Order(Customerid,customer,OrderStatus.Draft, _pricerule,_shippingfee,_OrderDiscount,_tax,IsShipping);
            NewOrder.OrderDate = DateTime.Now;
            if (NewOrder.IsShipping)
            {
                Console.Write("Vui lòng nhập địa chỉ cần giao: ");
                NewOrder.Adress = int.Parse(Console.ReadLine());
            }
            NewOrder._OrderChangeStatus += (s, e) =>
            {
                Console.WriteLine($"[EVENT] Order's old status {e.old} -> Order's new status {e.newstatus}");
            };
            _repoOrder.Add(NewOrder);
        }
        public void AddLine(string Orderid,string ProductID,int quantity,CakeSize size,Topping topping)
        {
            Order order = _repoOrder.GetById(Orderid);
            if (order == null) throw new InvalidOperationException("Order not found!!");
            CakeProduct product = _repoProduct.GetById(ProductID);
            if (product == null) throw new InvalidOperationException("Product not found!!");

            bool IsReward = (quantity >= 10) ? true : false;
            order.AddLine(new OrderLine(Orderid, product, size, topping, quantity, IsReward));
            
            _repoOrder.Update(Orderid, order);
        }
        public void Confirmed(string OrderID)
        {
            Order order = _repoOrder.GetById(OrderID);
            if (order == null) throw new InvalidOperationException("Order not found!!");
            if (order.status != OrderStatus.Draft) throw new InvalidOperationException("Only Draft can be Confirmed!!");

            foreach (var line in order.lines)
            {
                InService.DecreaseStock(line.product.ProductId, line.Quantity);
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
            _Promotion.ApplyPromotionRule(order);
            Console.WriteLine($"{payment} - {DateTime.Now} - {OrderID}");
            Payment NewPay = new Payment(payment, DateTime.Now, OrderID, order);
            _repoPayment.Add(NewPay);
            order.ChangeStatus(OrderStatus.Paid);

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
