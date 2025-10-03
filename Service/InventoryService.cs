using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Domain;
using QuanLyCuaHangBanhNgot_BanhKem.Generic;
using QuanLyCuaHangBanhNgot_BanhKem.Transaction;
namespace QuanLyCuaHangBanhNgot_BanhKem.Service
{
    
    public class InventoryService
    {
        InMemoryInventory<CakeProduct, string> inventory = new InMemoryInventory<CakeProduct, string>();
        public event EventHandler<InventoryLowEventArgs> Islow; 
        public void CheckStock(string id)
        {
            CakeProduct item = inventory.GetById(id);
            if (item != null)
            {
                if (item.IsLowStock()) Islow?.Invoke(this, new InventoryLowEventArgs(item));
            }
        }
        public void DecreaseStock(string id, int amount)
        {
            TransactionService trans = new TransactionService();
            CakeProduct item = inventory.GetById(id);
            if (item == null) {
                throw new InvalidOperationException("There are no products matching the ID!!");
            }
            else if (item.StockQty < amount) {
                throw new InvalidOperationException("Not enough stock!!");
            }

            item.DeductStock(amount);
            if(item.IsLowStock()) Islow?.Invoke(this, new InventoryLowEventArgs(item));

            trans.AddTransaction(new InventoryTransaction(item, amount, "DS-T01"));
        }

        public void ReStock(string id, int amount)
        {
            TransactionService trans = new TransactionService();
            CakeProduct item = inventory.GetById(id);
            if (item == null)
            {
                throw new InvalidOperationException("There are no products matching the ID!!");
            }
            item.StockQty = amount;
            inventory.Update(id, item);

            trans.AddTransaction(new InventoryTransaction(item, amount, "RS-T02"));
        }
    }
}
