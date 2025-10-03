using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
            IPriceRule _pricerule = new DefaultPriceRule();
            IOrderDiscountPolicy _discountOrder = new SpendBaseDiscountPolicy();
            IShippingFee _shipping = new InnerCityShipping();
            ITaxCaculators _taxCal = new VATTax.TenVATPercent();
            IPromotionRule<Order> _promotionRule = new Buy10Get1Free(null);

            
            //khởi tạo menu:
            InMemoryInventory<CakeProduct, string> menu = new InMemoryInventory<CakeProduct, string>();
            InMemoryInventory<Customer, string> _repoCustomer = new InMemoryInventory<Customer, string>();
            InMemoryInventory<Order, string> _repoOrder = new InMemoryInventory<Order, string>();
            InMemoryInventory<Payment, string> _repoPayment = new InMemoryInventory<Payment, string>();
            int ReorderThreshold = 10;
            var _InService = new InventoryService(menu,ReorderThreshold);
            var _OrService = new OrderService(_pricerule, _discountOrder, _taxCal, _promotionRule, _shipping,menu,_repoOrder,_repoCustomer,_repoPayment);
            
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
            _InService.Islow += (s, e) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[EVENT] Low Stock: {e.product.ProductId} - Qty: {e.product.StockQty}");
                Console.ResetColor();
            };
            _OrService._OrderChangeStatus += (s, e) =>
            {
                Console.WriteLine($"[EVENT] Order's old status {e.old} -> Order's new status {e.newstatus}");
            };
            _OrService.AddPoint += (s, e) =>
            {
                Console.WriteLine($"[Event] Points reward {e.points} -> Total points {e.total}");
            };
            menu.Add(c1_1);
            menu.Add(c1_2);
            menu.Add(c2_1);
            menu.Add(c2_2);


            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("======= 🧁 CỬA HÀNG BÁNH MÌ & BÁNH KEM =======");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1. ➕ Thêm sản phẩm");
                Console.WriteLine("2. ✏️ Cập nhật sản phẩm");
                Console.WriteLine("3. 🗑️ Xóa sản phẩm");
                Console.WriteLine("4. 📋 Danh sách sản phẩm");
                Console.WriteLine("5. 🛒 Tạo đơn hàng cho khách");
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

                    if (res < 0 || res > 5)
                        throw new Exception("❌ Vui lòng nhập từ [0 -> 4]!");

                    switch (res)
                    {
                        case (1):

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("=== ➕ Nhập sản phẩm mới ===");
                            Console.ResetColor();

                            CakeProduct newCake = new CakeProduct();


                            Console.Write("👉 Nhập mã sản phẩm: ");
                            newCake.ProductId = Console.ReadLine();


                            Console.Write("👉 Nhập tên sản phẩm: ");
                            newCake.Name = Console.ReadLine();


                            Console.Write("👉 Nhập loại bánh (Cake = 0, Bread = 1): ");
                            int type;
                            if (int.TryParse(Console.ReadLine(), out type) && Enum.IsDefined(typeof(CakeType), type))
                            {
                                newCake.caketype = (CakeType)type;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("❌ Loại bánh không hợp lệ, mặc định là Cake.");
                                Console.ResetColor();
                                newCake.caketype = CakeType.Cake;
                            }


                            Console.Write("👉 Nhập đơn giá: ");
                            newCake.UnitPrice = decimal.Parse(Console.ReadLine() ?? "0");


                            Console.Write("👉 Nhập số lượng tồn kho: ");
                            newCake.StockQty = int.Parse(Console.ReadLine() ?? "0");


                            Console.Write("👉 Sản phẩm có còn bán không? (y/n): ");
                            string active = Console.ReadLine();
                            newCake.IsActive = active?.ToLower() == "y";

                            menu.Add(newCake);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n=== ✅ Thông tin sản phẩm vừa nhập ===");
                            Console.ResetColor();
                            Console.WriteLine($"Mã SP: {newCake.ProductId}");
                            Console.WriteLine($"Tên SP: {newCake.Name}");
                            Console.WriteLine($"Loại bánh: {newCake.caketype}");
                            Console.WriteLine($"Đơn giá: {newCake.UnitPrice} đ");
                            Console.WriteLine($"Số lượng tồn: {newCake.StockQty}");
                            Console.WriteLine($"Trạng thái: {(newCake.IsActive ? "Đang bán" : "Ngừng bán")}");

                            _InService.CheckStock(newCake.ProductId);

                            Console.ReadKey();
                            break;
                        case (2):
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("=== ✏️ Cập nhật sản phẩm  ===");
                            Console.ResetColor();

                            CakeProduct newCake1 = new CakeProduct();


                            Console.Write("👉 Nhập mã sản phẩm: ");
                            newCake1.ProductId = Console.ReadLine();

                            if (menu.GetById(newCake1.ProductId) == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                throw new InvalidOperationException("Mã sản phẩm không hợp lệ vui lòng kiểm tra lại!!");
                            }
                            Console.Write("👉 Nhập tên sản phẩm: ");
                            newCake1.Name = Console.ReadLine();


                            Console.Write("👉 Nhập loại bánh (Cake = 0, Bread = 1): ");
                            int type1;
                            if (int.TryParse(Console.ReadLine(), out type1) && Enum.IsDefined(typeof(CakeType), type1))
                            {
                                newCake1.caketype = (CakeType)type1;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("❌ Loại bánh không hợp lệ, mặc định là Cake.");
                                Console.ResetColor();
                                newCake1.caketype = CakeType.Cake;
                            }


                            Console.Write("👉 Nhập đơn giá: ");
                            newCake1.UnitPrice = decimal.Parse(Console.ReadLine() ?? "0");


                            Console.Write("👉 Nhập số lượng tồn kho: ");
                            newCake1.StockQty = int.Parse(Console.ReadLine() ?? "0");


                            Console.Write("👉 Sản phẩm có còn bán không? (y/n): ");
                            string active1 = Console.ReadLine();
                            newCake1.IsActive = active1?.ToLower() == "y";

                            
                            menu.Update(newCake1.ProductId, newCake1);


                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n=== ✏️ Thông tin sản phẩm vừa thay đổi ===");
                            Console.ResetColor();
                            Console.WriteLine($"Mã SP: {newCake1.ProductId}");
                            Console.WriteLine($"Tên SP: {newCake1.Name}");
                            Console.WriteLine($"Loại bánh: {newCake1.caketype}");
                            Console.WriteLine($"Đơn giá: {newCake1.UnitPrice} đ");
                            Console.WriteLine($"Số lượng tồn: {newCake1.StockQty}");
                            Console.WriteLine($"Trạng thái: {(newCake1.IsActive ? "Đang bán" : "Ngừng bán")}");

                            _InService.CheckStock(newCake1.ProductId);

                            Console.ReadKey();
                            break;
                        case (3):
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("=== XÓA SẢN PHẨM ===");
                            Console.ResetColor();

                            
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nDanh sách sản phẩm hiện có:");
                            Console.ResetColor();

                            foreach (var sp in menu.GetAll())
                            {
                                Console.WriteLine($"{sp.ProductId,-8} | {sp.Name,-30} | {sp.UnitPrice,10} | SL: {sp.StockQty}");
                            }

                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("\nNhập mã sản phẩm cần xóa: ");
                            Console.ResetColor();
                            string idDel = Console.ReadLine();

                            var product = menu.GetById(idDel);
                            if (product == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n❌ Không tìm thấy sản phẩm với mã đã nhập!");
                                Console.ResetColor();
                            }
                            else
                            {
                                menu.Remove(idDel);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\n✔ Đã xóa sản phẩm [{product.ProductId}] - {product.Name}");
                                Console.ResetColor();
                            }

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\nNhấn phím bất kỳ để quay lại menu chính...");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                        case (4):
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("===========================================");
                            Console.WriteLine("       📋 DANH SÁCH SẢN PHẨM");
                            Console.WriteLine("===========================================");
                            Console.ResetColor();

                            if (menu == null || menu.inventory.Count == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("❌ Hiện chưa có sản phẩm nào trong kho!");
                                Console.ResetColor();
                            }
                            else
                            {

                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"{"Mã SP",-8} {"Tên sản phẩm",-30} {"Loại",-8} {"Giá",-10} {"SL",-5} {"Trạng thái",-10}");
                                Console.WriteLine(new string('-', 75));
                                Console.ResetColor();


                                foreach (var sp in menu.inventory)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    string trangThai = sp.IsActive ? "Đang bán" : "Ngừng";
                                    Console.WriteLine($"{sp.ProductId,-8} {sp.Name,-30} {sp.caketype,-8} {sp.UnitPrice,-10} {sp.StockQty,-5} {trangThai,-10}");
                                    Console.ResetColor();
                                }
                            }

                            Console.WriteLine("===========================================");
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write("👉 Nhấn phím bất kỳ để quay lại menu...");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                        default:
                            break;
                    }
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
