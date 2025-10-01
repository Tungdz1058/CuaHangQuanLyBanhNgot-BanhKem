using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Domain;
using QuanLyCuaHangBanhNgot_BanhKem.Generic;
using QuanLyCuaHangBanhNgot_BanhKem.Pricing;
using QuanLyCuaHangBanhNgot_BanhKem.Service;
using QuanLyCuaHangBanhNgot_BanhKem.Receipts;
namespace QuanLyCuaHangBanhNgot_BanhKem.Transaction
{
    public class OrderTransaction : ITransaction
    {
        public string TransactionID { get; private set; }

        private readonly IReceiptFormatter _Receipt;
        public OrderTransaction(string id,IReceiptFormatter receipt)
        {
            this.TransactionID = id;
            this._Receipt = receipt;
        }

        public void Print()
        {
            _Receipt.PrintInfor();
        }
    }
}
