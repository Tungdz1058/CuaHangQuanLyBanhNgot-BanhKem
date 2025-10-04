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
        private readonly IRepository<CakeProduct, string> _productRepo;
        public event EventHandler<InventoryLowEventArgs> Islow;
        TransactionService trans;
        public int ReorderThreshold { get; private set; }
        public InventoryService(IRepository<CakeProduct,string> repo,int ReorderThreshold,TransactionService trans)
        {
            this._productRepo = repo;
            this.ReorderThreshold = ReorderThreshold;
            this.trans = trans;
        }
        public void CheckStock(string id)
        {
            CakeProduct item = _productRepo.GetById(id);
            if (item != null)
            {
                if (item.StockQty < ReorderThreshold)
                {
                    Islow?.Invoke(this, new InventoryLowEventArgs(item));
                }
            }
        }
        public void DecreaseStock(string id, int amount)
        {
            
            CakeProduct item = _productRepo.GetById(id);
            if (item == null) {
                throw new InvalidOperationException("There are no products matching the ID!!");
            }
            else if (item.StockQty < amount) {
                throw new InvalidOperationException("Not enough stock!!");
            }

            item.DeductStock(amount);
            if(item.StockQty < ReorderThreshold) Islow?.Invoke(this, new InventoryLowEventArgs(item));

            trans.AddTransaction(new InventoryTransaction(item, amount, "DS-T01"));
        }

        public void ReStock(string id, int amount)
        {
            TransactionService trans = new TransactionService();
            CakeProduct item = _productRepo.GetById(id);
            if (item == null)
            {
                throw new InvalidOperationException("There are no products matching the ID!!");
            }
            item.StockQty = amount;
            _productRepo.Update(id, item);

            trans.AddTransaction(new InventoryTransaction(item, amount, "RS-T02"));
        }
    }
}
