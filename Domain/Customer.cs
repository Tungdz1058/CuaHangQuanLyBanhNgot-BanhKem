using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyCuaHangBanhNgot_BanhKem.Generic;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public abstract class Customer : IEntity<string>
    {
        public string CustomerID { get; private set; }
        public string FullName { get; private set; }
        private string phone;
            public string Phone
            {
                get { return phone; }
                private set
                {
                    try
                    {
                        if (String.IsNullOrWhiteSpace(value)) throw new Exception();
                        for(int i = 0; i < value.Length; i++)
                        {
                            if (Char.IsLetter(value[i])) throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Số điện thoại không hợp lệ vui lòng nhập lại!!!");
                    }
                phone = value;
                }
            }

        public string ID => CustomerID;

        public Customer(string id, string name, string number)
        {
            this.CustomerID = id;
            this.FullName = name;
            this.Phone = number;

        }
        public abstract decimal GetDiscountPercent();
        public virtual bool CanAccruePoints() => false;
        public virtual void AddPoints(decimal pts) { }
    }
}
