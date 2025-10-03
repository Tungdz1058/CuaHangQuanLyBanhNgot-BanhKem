using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

            var member1 = new MemberCustomer("MEM-01","Lê Đình Tùng", "0563090775", "Memberedx", 0, MemberTier.Standard);
            _repoCustomer.Add(member1);


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

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"{"Mã SP",-8} {"Tên sản phẩm",-30} {"Loại",-8} {"Giá",-10} {"SL",-5} {"Trạng thái",-10}");
                            Console.WriteLine(new string('-', 75));
                            Console.ResetColor();

                            foreach (var sp in menu.inventory)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                string trangThai = sp.IsActive ? "Đang bán" : "Ngừng bán";
                                Console.WriteLine($"{sp.ProductId,-8} {sp.Name,-30} {sp.caketype,-8} {sp.UnitPrice,-10} {sp.StockQty,-5} {trangThai,-10}");
                                Console.ResetColor();
                            }

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
                                    string trangThai = sp.IsActive ? "Đang bán" : "Ngừng bán";
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
                        case (5):
                            bool backOrderMenu = false;
                            while (!backOrderMenu)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("=== QUẢN LÝ ĐƠN HÀNG ===");
                                Console.ResetColor();

                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("1. 🛒 Tạo đơn hàng");
                                Console.WriteLine("2. 💳 Tiến hành thanh toán");
                                Console.WriteLine("3. 🧾 Xuất hóa đơn");
                                Console.WriteLine("0. 🔙 Quay lại menu chính");
                                Console.ResetColor();

                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                Console.Write("\n👉 Chọn chức năng: ");
                                Console.ResetColor();
                                string ch5 = Console.ReadLine() ?? "0";

                                switch (ch5)
                                {
                                    case "1":
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.WriteLine("=== 🛒 TẠO ĐƠN HÀNG ===");
                                        Console.ResetColor();

                                        // B1: chọn khách hàng
                                        Console.WriteLine("\n1. Thành viên");
                                        Console.WriteLine("2. Vãng lai");
                                        Console.Write("👉 Chọn loại khách hàng: ");
                                        string custType = Console.ReadLine();
                                        Customer customer;
                                        string ID = "WALK-01";
                                        if(custType == "1")
                                        {
                                            Console.WriteLine("Vui lòng nhập mã khách hàng: ");
                                            ID = Console.ReadLine();
                                            customer = _repoCustomer.GetById(ID);
                                            if (customer == null) throw new InvalidOperationException("Mã khách hàng không tìm thấy!!");
                                        }else if(custType == "2")
                                        {
                                            Console.Write("Vui lòng nhập tên: ");
                                            string name = Console.ReadLine();
                                            Console.Write("Vui lòng nhập số điện thoại: ");
                                            string phonenum = Console.ReadLine();
                                            customer = new WalkInCustomer(ID, name,phonenum);
                                        }

                                        // B2: chọn sản phẩm
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.WriteLine($"{"Mã SP",-8} {"Tên sản phẩm",-30} {"Loại",-8} {"Giá",-10} {"SL",-5}");
                                        Console.WriteLine(new string('-', 63));
                                        Console.ResetColor();
                                        foreach (var sp in menu.inventory)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine($"{sp.ProductId,-8} {sp.Name,-30} {sp.caketype,-8} {sp.UnitPrice,-10} {sp.StockQty,-5}");
                                            Console.ResetColor();
                                        }
                                        bool Ishipping = false;
                                        Console.WriteLine("Vui lòng chọn (1 . Thanh toán tại quầy, 2 . Thanh toán khi giao hàng)");
                                        string input1 = Console.ReadLine();
                                        if (input1 == "2") Ishipping = true;
                                        else if (input1 != "1" && input1 != "2") throw new InvalidOperationException("dữ liệu nhập không hợp lệ!!");
                                        _OrService.CreateOrder(ID,Ishipping);

                                        Console.Write("Nhập số loại bánh muốn mua: ");
                                        int quantity =int.Parse(Console.ReadLine());
                                        while (quantity >0)
                                        {
                                            quantity -= 1;
                                            Console.Write("👉 Nhập mã sản phẩm cần thêm vào giỏ: ");
                                            string productID = Console.ReadLine();
                                            Console.Write("Nhập số lượng: ");
                                            int qty = int.Parse(Console.ReadLine());
                                            Console.Write("👉 Nhập size bánh (S = 0, M = 1, L = 2): ");
                                            int type2;
                                            CakeSize size;
                                            if (int.TryParse(Console.ReadLine(), out type2) && Enum.IsDefined(typeof(CakeSize), type2))
                                            {
                                                size = (CakeSize)type2;
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("❌ Size bánh không hợp lệ, mặc định là S.");
                                                Console.ResetColor();
                                                size = CakeSize.S;
                                            }
                                            Console.Write("👉 Nhập topping bánh (cheese = 0, socola = 1, corn = 2, strawberry = 3): ");
                                            int type3;
                                            Topping topping;
                                            if (int.TryParse(Console.ReadLine(), out type3) && Enum.IsDefined(typeof(Topping), type3))
                                            {
                                                topping = (Topping)type3;
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine("❌ Topping bánh không hợp lệ, mặc định là socola.");
                                                Console.ResetColor();
                                                topping = Topping.socola;
                                            }
                                            _OrService.AddLine(ID, productID, qty, size, topping);
                                        }
                                        // B3: xác nhận đơn
                                        Console.WriteLine("=================================");
                                        Console.WriteLine("1. ✅ Xác nhận");
                                        Console.WriteLine("2. ❌ Hủy");
                                        Console.Write("\nXác nhận đơn hàng? Vui lòng nhập: ");
                                        string confirm = Console.ReadLine();
                                        if (confirm == "1")
                                        {
                                            Console.WriteLine("✔ Đơn hàng đã được tạo thành công!");
                                            // _repoOrder.Add(order);
                                        }
                                        else
                                        {
                                            Console.WriteLine("❌ Đơn hàng đã bị hủy.");
                                            _OrService.Cancelled(ID);
                                        }
                                        Console.ReadKey();
                                        break;

                                    case "2":
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.WriteLine("=== 💳 TIẾN HÀNH THANH TOÁN ===");
                                        Console.ResetColor();

                                        Console.Write("👉 Nhập mã đơn hàng cần thanh toán: ");
                                        string orderId = Console.ReadLine();

                                        // (Tìm đơn hàng từ _repoOrder)
                                        Console.WriteLine("Chọn phương thức thanh toán:");
                                        Console.WriteLine("1. Tiền mặt");
                                        Console.WriteLine("2. Thẻ");
                                        string payMethod = Console.ReadLine();

                                        Console.WriteLine("✔ Thanh toán thành công!");
                                        // _OrService.Pay(orderId, PaymentMethod.Cash/Card)

                                        Console.ReadKey();
                                        break;

                                    case "3":
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.WriteLine("=== 🧾 XUẤT HÓA ĐƠN ===");
                                        Console.ResetColor();

                                        Console.Write("👉 Nhập mã đơn hàng cần in hóa đơn: ");
                                        string billId = Console.ReadLine();

                                        // (Lấy Order từ _repoOrder và in ra chi tiết)
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("\n====== HÓA ĐƠN BÁNH NGỌT & BÁNH KEM ======");
                                        Console.ResetColor();

                                        // Giả lập in bill
                                        Console.WriteLine($"Mã đơn: {billId}");
                                        Console.WriteLine($"Ngày: {DateTime.Now}");
                                        Console.WriteLine("----------------------------------------");
                                        Console.WriteLine("SP01   | Bánh mì bơ tỏi   | 20.000 x2 = 40.000");
                                        Console.WriteLine("SP02   | Bánh kem trứng   | 150.000 x1 = 150.000");
                                        Console.WriteLine("----------------------------------------");
                                        Console.WriteLine("Tạm tính: 190.000");
                                        Console.WriteLine("VAT 10%: 19.000");
                                        Console.WriteLine("Phí ship: 15.000");
                                        Console.WriteLine("Tổng cộng: 224.000");
                                        Console.WriteLine("========================================");

                                        Console.ReadKey();
                                        break;

                                    case "0":
                                        backOrderMenu = true;
                                        break;

                                    default:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("❌ Lựa chọn không hợp lệ!");
                                        Console.ResetColor();
                                        Console.ReadKey();
                                        break;
                                }
                            }
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
