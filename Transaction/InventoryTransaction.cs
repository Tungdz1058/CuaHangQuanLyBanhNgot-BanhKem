using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Domain; 
using QuanLyCuaHangBanhNgot_BanhKem.Generic; 

namespace QuanLyCuaHangBanhNgot_BanhKem.Transaction
{
    public class InventoryTransaction : ITransaction
    {
        public string TransactionID { get; private set; }
        public CakeProduct product { get; private set; }
        public DateTime date { get; private set; }
        public int ChangeStockAmount;
        public int CurrentStockAmount;
        public InventoryTransaction(CakeProduct product, int amount,string id)
        {
            this.product = product;
            TransactionID = id;
            date = DateTime.Now;
            ChangeStockAmount = amount;
            CurrentStockAmount = product.StockQty;
        }
        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║        📦  INVENTORY TRANSACTION LOG         ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine($"🆔 Transaction ID   : {TransactionID}");
            Console.WriteLine($"📅 Date             : {date:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"🍰 Product          : {product.Name} (ID: {product.ProductId})");

            if (ChangeStockAmount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"➕ Change Amount    : -{ChangeStockAmount}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"➖ Change Amount    : {ChangeStockAmount}");
            }
            Console.ResetColor();

            Console.WriteLine($"📦 Current Stock    : {CurrentStockAmount}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("════════════════════════════════════════════════");
            Console.ResetColor();

        }
    }
}
