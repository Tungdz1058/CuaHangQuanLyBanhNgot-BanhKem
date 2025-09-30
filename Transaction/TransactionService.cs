using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Transaction
{
    public class TransactionService
    {
        List<ITransaction> alltransaction = new List<ITransaction>();
        public void AddTransaction(ITransaction log)
        {
            alltransaction.Add(log);
        }
        public void PrintAll()
        {
            foreach(var transaction in alltransaction)
            {
                transaction.Print();
            }
        }
    }
}
