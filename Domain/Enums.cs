using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public enum CakeSize { S, M, L };
    public enum Topping { cheese, socola, corn, strawberry };
    public enum OrderStatus { Draft, Confirmed, Paid, Shipped, Delivered, Cancelled };
    public enum MemberTier { Standard = 1, Silver = 2, Gold = 3};
    public enum PaymentMethod { Cash, BankTransfer};
    public enum CakeType { Bread, Cake };
}
