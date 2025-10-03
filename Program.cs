using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Domain;
using QuanLyCuaHangBanhNgot_BanhKem.Generic;
using QuanLyCuaHangBanhNgot_BanhKem.Pricing;
using QuanLyCuaHangBanhNgot_BanhKem.Promotion_rule;
using QuanLyCuaHangBanhNgot_BanhKem.Receipts;
using QuanLyCuaHangBanhNgot_BanhKem.Service;

namespace QuanLyCuaHangBanhNgot_BanhKem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            //khởi tạo menu:
            InMemoryInventory<CakeProduct, string> menu = new InMemoryInventory<CakeProduct, string>();
            InMemoryInventory<Customer, string> _repoCustomer = new InMemoryInventory<Customer, string>();
            InMemoryInventory<Order, string> _repoOrder = new InMemoryInventory<Order, string>();
            var c1_1 = new CakeProduct
            {
                ProductId = "B-01",
                Name = "Bánh mì bơ tỏi",
                caketype = CakeType.Bread,
                UnitPrice = 20m,
                StockQty = 50,
                IsActive = true,
            };
            var c1_2 = new CakeProduct
            {
                ProductId = "B-02",
                Name = "Bánh mì nướng mỡ hành",
                caketype = CakeType.Bread,
                UnitPrice = 25m,
                StockQty = 30,
                IsActive = true,
            };
            var c2_1 = new CakeProduct
            {
                ProductId = "C-01",
                Name = "Bánh kem ngọt truyền thống",
                caketype = CakeType.Cake,
                UnitPrice = 100m,
                StockQty = 40,
                IsActive = true,
            };
            var c2_2 = new CakeProduct
            {
                ProductId = "C-02",
                Name = "Bánh kem bông lan trứng muối",
                caketype = CakeType.Cake,
                UnitPrice = 150m,
                StockQty = 20,
                IsActive = true,
            };
            menu.Add(c1_1);
            menu.Add(c1_2);
            menu.Add(c2_1);
            menu.Add(c2_2);


            IPriceRule _pricerule = new DefaultPriceRule();
            IOrderDiscountPolicy _discountOrder = new SpendBaseDiscountPolicy();
            IShippingFee _shipping = new InnerCityShipping();
            ITaxCaculators _taxCal = new VATTax.TenVATPercent();
            IPromotionRule<Order> _promotionRule = new Buy10Get1Free(null);

            var _InService = new InventoryService();
            var _OrService = new OrderService(_pricerule,_discountOrder,_taxCal,_promotionRule,_shipping);

            _InService.Islow += (s, e) =>
            {
                Console.WriteLine($"[EVENT] Low Stock: {e.product.ID} - Qty: {e.product.StockQty}");
            };
            _OrService._OrderChangeStatus += (s, e) =>
            {
                Console.WriteLine($"[EVENT] Order's old status {e.old} -> Order's new status {e.newstatus}");
            };
            _OrService.AddPoint += (s, e) =>
            {
                Console.WriteLine($"Points reward {e.points} -> Total points {e.total}");
            };


            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("======= 🧁 CỬA HÀNG BÁNH MÌ & BÁNH KEM =======");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1. ➕ Thêm sản phẩm");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("2. ✏️ Cập nhật sản phẩm");
                Console.WriteLine("3. 🗑️ Xóa sản phẩm");
                Console.WriteLine("4. 📋 Danh sách sản phẩm");
                Console.WriteLine("0. 🔙 Quay lại menu chính");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write("👉 Chọn chức năng: ");
                Console.ResetColor();
                try
                {
                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out int res))
                        throw new Exception("❌ Dữ liệu nhập không hợp lệ, vui lòng nhập lại!");

                    if (res < 0 || res > 4)
                        throw new Exception("❌ Vui lòng nhập từ [0 -> 4]!");

                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                    Console.WriteLine("Ấn phím bất kỳ để thử lại...");
                    Console.ReadKey();
                }

            }
        }
    }
}
